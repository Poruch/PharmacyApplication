
namespace TreeViewCrud.Models;

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

    public int LogId
    {
        get => _logId;
        set => _logId = value;
    }

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

    public int? RecordId
    {
        get => _recordId;
        set => _recordId = value;
    }

    public string? OldValue
    {
        get => _oldValue;
        set => _oldValue = value;
    }

    public string? NewValue
    {
        get => _newValue;
        set => _newValue = value;
    }

    public DateTime Timestamp
    {
        get => _timestamp;
        set => _timestamp = value;
    }

    public string? IpAddress
    {
        get => _ipAddress;
        set => _ipAddress = value;
    }

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