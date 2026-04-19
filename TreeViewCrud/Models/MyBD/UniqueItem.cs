
namespace TreeViewCrud.Models;
public class UniqueItem
{
    private int _uniqueItemId;
    private string _codeMarking = null!;
    private string _status = null!;
    private int _batchId;

    public int UniqueItemId
    {
        get => _uniqueItemId;
        set => _uniqueItemId = value;
    }

    public string CodeMarking
    {
        get => _codeMarking;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Code marking cannot be empty");
            _codeMarking = value;
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            var allowed = new[] { "in_stock", "sold", "returned", "written_off" };
            if (!allowed.Contains(value))
                throw new ArgumentException($"Status must be one of: {string.Join(", ", allowed)}");
            _status = value;
        }
    }

    public int BatchId
    {
        get => _batchId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("BatchId must be positive");
            _batchId = value;
        }
    }

    public UniqueItem() { }

    public UniqueItem(string codeMarking, string status, int batchId)
    {
        CodeMarking = codeMarking;
        Status = status;
        BatchId = batchId;
    }
}