using Microsoft.Data.SqlClient;
using TreeViewCrud.Models;

public class EntityRepository
{
    private readonly string _connectionString;

    public EntityRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int Add<T>(T entity) where T : EntityBase
    {
        var (sql, parameters) = entity.GenerateInsertSql();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        foreach (var p in parameters)
            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
        conn.Open();
        int newId = Convert.ToInt32(cmd.ExecuteScalar());
        entity.Id = newId;
        return newId;
    }

    public void Update<T>(T entity) where T : EntityBase
    {
        var (sql, parameters) = entity.GenerateUpdateSql();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        foreach (var p in parameters)
            cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void Delete<T>(T entity) where T : EntityBase
    {
        string sql = entity.GenerateDeleteSql();
        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        conn.Open();
        cmd.ExecuteNonQuery();
    }

    public void DeleteById<T>(int id) where T : EntityBase, new()
    {
        var dummy = new T { Id = id };
        Delete(dummy);
    }

    public List<T> GetAll<T>() where T : EntityBase, new()
    {
        var list = new List<T>();
        var dummy = new T();
        string sql = $"SELECT * FROM {dummy.GenerateDeleteSql().Split(' ')[2]}";
        string tableName = dummy.GetTableName();

        using var conn = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand($"SELECT * FROM {tableName}", conn);
        conn.Open();
        using var reader = cmd.ExecuteReader();
        var properties = typeof(T).GetProperties().ToDictionary(p => p.Name);
        while (reader.Read())
        {
            var entity = new T();
            foreach (var prop in typeof(T).GetProperties())
            {
                if (reader[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(entity, Convert.ChangeType(reader[prop.Name], prop.PropertyType));
                }
            }
            list.Add(entity);
        }
        return list;
    }
}