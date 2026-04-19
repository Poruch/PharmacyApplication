
namespace TreeViewCrud.Models;
public class StorageLocation
{
    private int _locationId;
    private string _shelf = null!;
    private string? _cell;

    public int LocationId
    {
        get => _locationId;
        set => _locationId = value;
    }

    public string Shelf
    {
        get => _shelf;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Shelf cannot be empty");
            _shelf = value;
        }
    }

    public string? Cell
    {
        get => _cell;
        set => _cell = value;
    }

    public StorageLocation() { }

    public StorageLocation(string shelf, string? cell = null)
    {
        Shelf = shelf;
        Cell = cell;
    }
}