using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
namespace TreeViewCrud.Models;
public abstract class EntityBase
{
    public int Id { get; set; }

    public string GetTableName()
    {
        var tableAttr = GetType().GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name ?? GetType().Name + "s";
    }
    public static string GetTableName<T>() where T : EntityBase
    {
        var tableAttr = typeof(T).GetCustomAttribute<TableAttribute>();
        return tableAttr?.Name ?? typeof(T).Name + "s";
    }

    private List<PropertyInfo> GetNonKeyProperties()
    {
        return GetType().GetProperties()
            .Where(p => !p.IsDefined(typeof(KeyAttribute)) && p.Name != "Id")
            .ToList();
    }

    private string GetColumnName(PropertyInfo prop)
    {
        var colAttr = prop.GetCustomAttribute<ColumnAttribute>();
        return colAttr?.Name ?? prop.Name;
    }

    public (string sql, Dictionary<string, object?> parameters) GenerateInsertSql()
    {
        var nonKeyProps = GetNonKeyProperties();
        var columns = nonKeyProps.Select(p => GetColumnName(p));
        var paramNames = nonKeyProps.Select(p => "@" + p.Name);

        var sql = $@"
            INSERT INTO {GetTableName()} ({string.Join(", ", columns)})
            VALUES ({string.Join(", ", paramNames)});
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        var parameters = new Dictionary<string, object?>();
        foreach (var prop in nonKeyProps)
        {
            parameters.Add("@" + prop.Name, prop.GetValue(this));
        }
        return (sql, parameters);
    }
    public (string sql, Dictionary<string, object?> parameters) GenerateUpdateSql()
    {
        var nonKeyProps = GetNonKeyProperties();
        var setClauses = nonKeyProps.Select(p => $"{GetColumnName(p)} = @{p.Name}");

        var sql = $@"
            UPDATE {GetTableName()}
            SET {string.Join(", ", setClauses)}
            WHERE Id = @Id";

        var parameters = new Dictionary<string, object?> { ["@Id"] = Id };
        foreach (var prop in nonKeyProps)
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