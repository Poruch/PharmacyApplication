
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TreeViewCrud.Models;
[Table("AuditLog")]
public class AuditLog
{
    private int _logId;
    private string _action = null!;
    private string _tableName = null!;
    private int? _recordId;
    private string? _oldValue;
    private string? _newValue;
    private DateTime _timestamp;
    private string? _ipAddress;
    private int _userId;
    [Column("LogId")]
    [Key]
    public int LogId
    {
        get => _logId;
        set => _logId = value;
    }
    [Column("Action")]

    public string Action
    {
        get => _action;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Action cannot be empty");
            _action = value;
        }
    }
    [Column("TableName")]

    public string TableName
    {
        get => _tableName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Table name cannot be empty");
            _tableName = value;
        }
    }
    [Column("RecordId")]

    public int? RecordId
    {
        get => _recordId;
        set => _recordId = value;
    }
    [Column("OldValue")]

    public string? OldValue
    {
        get => _oldValue;
        set => _oldValue = value;
    }
    [Column("NewValue")]

    public string? NewValue
    {
        get => _newValue;
        set => _newValue = value;
    }
    [Column("Timestamp")]

    public DateTime Timestamp
    {
        get => _timestamp;
        set => _timestamp = value;
    }
    [Column("IpAddress")]

    public string? IpAddress
    {
        get => _ipAddress;
        set => _ipAddress = value;
    }
    [Column("UserId")]

    public int UserId
    {
        get => _userId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("UserId must be positive");
            _userId = value;
        }
    }

    public AuditLog() { }

    public AuditLog(string action, string tableName, int userId, DateTime? timestamp = null,
                    int? recordId = null, string? oldValue = null, string? newValue = null, string? ipAddress = null)
    {
        Action = action;
        TableName = tableName;
        UserId = userId;
        RecordId = recordId;
        OldValue = oldValue;
        NewValue = newValue;
        IpAddress = ipAddress;
        Timestamp = timestamp ?? DateTime.Now;
    }
}