using System.Reflection;
using System.Text;
using TreeViewCrud.Attributes;

namespace TreeViewCrud.Models
{
    public abstract class EntityBase
    {
        public int Id { get; set; }

        public string GetTableName()
        {
            var tableAttr = GetType().GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? GetType().Name;
        }

        public static string GetTableName<T>() where T : EntityBase
        {
            var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
            return tableAttr?.Name ?? typeof(T).Name;
        }
        private List<PropertyInfo> GetColumnProperties()
        {
            return GetType().GetProperties()
                .Where(p => p.IsDefined(typeof(ColumnAttribute), false))
                .Where(p => !IsKeyProperty(p))
                .ToList();
        }

        private bool IsKeyProperty(PropertyInfo prop)
        {
            if (prop.IsDefined(typeof(KeyAttribute), false))
                return true;
            if (prop.Name == "Id")
                return true;
            return false;
        }

        private string GetColumnName(PropertyInfo prop)
        {
            var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
            return colAttr?.Name ?? prop.Name;
        }

        public (string sql, Dictionary<string, object?> parameters) GenerateInsertSql()
        {
            var columnProps = GetColumnProperties();
            var columns = columnProps.Select(p => GetColumnName(p));
            var paramNames = columnProps.Select(p => "@" + p.Name);

            var sql = $@"
                INSERT INTO {GetTableName()} ({string.Join(", ", columns)})
                VALUES ({string.Join(", ", paramNames)});
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new Dictionary<string, object?>();
            foreach (var prop in columnProps)
            {
                parameters.Add("@" + prop.Name, prop.GetValue(this));
            }
            return (sql, parameters);
        }

        public (string sql, Dictionary<string, object?> parameters) GenerateUpdateSql()
        {
            var columnProps = GetColumnProperties();
            var setClauses = columnProps.Select(p => $"{GetColumnName(p)} = @{p.Name}");

            var sql = $@"
                UPDATE {GetTableName()}
                SET {string.Join(", ", setClauses)}
                WHERE Id = @Id";

            var parameters = new Dictionary<string, object?> { ["@Id"] = Id };
            foreach (var prop in columnProps)
            {
                parameters.Add("@" + prop.Name, prop.GetValue(this));
            }
            return (sql, parameters);
        }

        public string GenerateDeleteSql()
        {
            return $"DELETE FROM {GetTableName()} WHERE Id = @Id";
        }
    }
}