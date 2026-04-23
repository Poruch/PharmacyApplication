using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Microsoft.Data.SqlClient;
using TreeViewCrud.Models;
namespace TreeViewCrud.Services
{
    public static class DatabaseInitializer
    {
        private static readonly string TargetConnectionString = ConfigManager.ConnectionString;

        /// <summary>
        /// Создаёт все таблицы, если они не существуют, в правильном порядке (с учётом внешних ключей)
        /// </summary>
        public static void EnsureDatabaseCreated()
        {
            CreateAllTablesOrdered();
        }

        /// <summary>
        /// Удаляет все таблицы (для сброса базы данных)
        /// </summary>
        public static void DropAllTables()
        {
            using (var connection = new SqlConnection(TargetConnectionString))
            {
                connection.Open();
                var dropScript = @"
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AuditLog') DROP TABLE AuditLog;
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Batch') DROP TABLE Batch;
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Item') DROP TABLE Item;
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'AppUser') DROP TABLE AppUser;
                    IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Category') DROP TABLE Category;
                ";
                using (var cmd = new SqlCommand(dropScript, connection))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Создаёт таблицы для всех указанных типов в правильном порядке (сначала таблицы, на которые нет ссылок)
        /// </summary>
        public static void CreateAllTablesOrdered()
        {
            var modelTypes = new List<Type>
            {
                typeof(Category),
                typeof(Item),
                typeof(Batch),
                typeof(AppUser),
                typeof(AuditLog)
            };
            var ordered = TopologicalSort(modelTypes);
            foreach (var type in ordered)
            {
                var method = typeof(DatabaseInitializer).GetMethod(nameof(CreateTableIfNotExists))
                    ?.MakeGenericMethod(type);
                method?.Invoke(null, null);
            }
        }

        /// <summary>
        /// Создаёт таблицу для типа T, если она не существует.
        /// Использует атрибуты [Table], [Column], [Key], [ForeignKey], [MaxLength].
        /// </summary>
        public static void CreateTableIfNotExists<T>() where T : class, new()
        {
            var type = typeof(T);
            string tableName = GetTableName(type);
            var columns = GetColumnDefinitions(type).ToList();
            var foreignKeys = GetForeignKeys(type).ToList();

            string createTableSql = $@"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = '{tableName}')
                BEGIN
                    CREATE TABLE [{tableName}] (
                        {string.Join(", ", columns)}
                    );
                END";

            using (var connection = new SqlConnection(TargetConnectionString))
            {
                connection.Open();
                using (var cmd = new SqlCommand(createTableSql, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                // Добавляем внешние ключи, если их ещё нет
                foreach (var fk in foreignKeys)
                {
                    string checkFkSql = $@"
                        IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = '{fk.ConstraintName}')
                        BEGIN
                            ALTER TABLE [{tableName}] 
                            ADD CONSTRAINT [{fk.ConstraintName}] 
                            FOREIGN KEY ([{fk.ForeignKeyColumn}]) 
                            REFERENCES [{fk.ReferencedTable}]([{fk.ReferencedColumn}])
                            ON DELETE CASCADE
                        END";
                    using (var cmd = new SqlCommand(checkFkSql, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        // ==================== Вспомогательные методы ====================

        private static string GetTableName(Type type)
        {
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? type.Name;
        }

        private static IEnumerable<string> GetColumnDefinitions(Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(ColumnAttribute)));

            foreach (var prop in props)
            {
                string columnName = prop.GetCustomAttribute<ColumnAttribute>()!.Name;
                string sqlType = MapCSharpTypeToSql(prop);
                string nullable = IsNullable(prop) ? "NULL" : "NOT NULL";
                string identity = IsAutoIncrementKey(prop) ? "IDENTITY(1,1)" : "";
                string primaryKey = IsPrimaryKey(prop) ? "PRIMARY KEY" : "";

                var parts = new[] { $"[{columnName}]", sqlType, nullable, identity, primaryKey }
                    .Where(p => !string.IsNullOrEmpty(p));
                yield return string.Join(" ", parts);
            }
        }

        private static string MapCSharpTypeToSql(PropertyInfo prop)
        {
            Type type = prop.PropertyType;
            bool isNullable = type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable)
                type = type.GetGenericArguments()[0];

            if (type == typeof(int) || type == typeof(short) || type == typeof(byte))
                return "INT";
            if (type == typeof(long))
                return "BIGINT";
            if (type == typeof(decimal))
                return "DECIMAL(18,2)";
            if (type == typeof(float))
                return "REAL";
            if (type == typeof(double))
                return "FLOAT";
            if (type == typeof(bool))
                return "BIT";
            if (type == typeof(DateTime))
                return "DATETIME2";
            if (type == typeof(DateTimeOffset))
                return "DATETIMEOFFSET";
            if (type == typeof(TimeSpan))
                return "TIME";
            if (type == typeof(Guid))
                return "UNIQUEIDENTIFIER";
            if (type == typeof(string))
            {
                var maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
                int length = maxLengthAttr?.Length ?? 255;
                return length == -1 || length > 4000 ? "NVARCHAR(MAX)" : $"NVARCHAR({length})";
            }
            if (type.IsEnum)
                return "INT";
            throw new NotSupportedException($"Тип {type.Name} не поддерживается для маппинга в SQL");
        }

        private static bool IsNullable(PropertyInfo prop)
        {
            // Для значимых типов проверяем Nullable<>
            if (prop.PropertyType.IsValueType)
                return Nullable.GetUnderlyingType(prop.PropertyType) != null;

            // Для ссылочных типов: если тип не помечен как "не null" (C# 8+), считаем nullable
            // В простом варианте возвращаем true для всех ссылочных типов
            return true;
        }

        private static bool IsPrimaryKey(PropertyInfo prop)
        {
            return prop.IsDefined(typeof(KeyAttribute));
        }

        private static bool IsAutoIncrementKey(PropertyInfo prop)
        {
            if (!IsPrimaryKey(prop)) return false;
            var type = prop.PropertyType;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
            return type == typeof(int) || type == typeof(long);
        }

        private class ForeignKeyInfo
        {
            public string ForeignKeyColumn { get; set; }
            public string ReferencedTable { get; set; }
            public string ReferencedColumn { get; set; }
            public string ConstraintName { get; set; }
        }

        private static IEnumerable<ForeignKeyInfo> GetForeignKeys(Type type)
        {
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.IsDefined(typeof(ForeignKeyAttribute)));

            string currentTable = GetTableName(type);

            foreach (var prop in properties)
            {
                var fkAttr = prop.GetCustomAttribute<ForeignKeyAttribute>();
                if (fkAttr == null) continue;

                // Имя навигационного свойства или типа
                string referencedName = fkAttr.Name;
                // Ищем тип по имени (в текущей сборке или сборках, где есть наши модели)
                Type referencedType = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .FirstOrDefault(t => t.Name == referencedName || t.FullName == referencedName);

                if (referencedType == null)
                {
                    // Возможно, указан не тип, а имя свойства – тогда игнорируем
                    continue;
                }

                string referencedTable = GetTableName(referencedType);
                string referencedColumn = GetPrimaryKeyColumn(referencedType);
                string columnName = prop.GetCustomAttribute<ColumnAttribute>()?.Name ?? prop.Name;
                string constraintName = $"FK_{currentTable}_{referencedTable}_{columnName}";

                yield return new ForeignKeyInfo
                {
                    ForeignKeyColumn = columnName,
                    ReferencedTable = referencedTable,
                    ReferencedColumn = referencedColumn,
                    ConstraintName = constraintName
                };
            }
        }

        private static string GetPrimaryKeyColumn(Type type)
        {
            var keyProp = type.GetProperties()
                .FirstOrDefault(p => p.IsDefined(typeof(KeyAttribute)));
            if (keyProp == null) return "Id"; // По умолчанию
            return keyProp.GetCustomAttribute<ColumnAttribute>()?.Name ?? keyProp.Name;
        }

        // ==================== Топологическая сортировка для FK зависимостей ====================

        private static List<Type> TopologicalSort(List<Type> types)
        {
            var graph = new Dictionary<Type, List<Type>>();
            foreach (var t in types)
            {
                graph[t] = new List<Type>();
                foreach (var fk in GetForeignKeys(t))
                {
                    var referencedType = types.FirstOrDefault(tt => GetTableName(tt) == fk.ReferencedTable);
                    if (referencedType != null)
                        graph[t].Add(referencedType);
                }
            }

            var sorted = new List<Type>();
            var visited = new HashSet<Type>();

            void Visit(Type node)
            {
                if (visited.Contains(node)) return;
                visited.Add(node);
                foreach (var dep in graph[node])
                    Visit(dep);
                sorted.Add(node);
            }

            foreach (var t in types)
                Visit(t);

            return sorted;
        }
    }
}