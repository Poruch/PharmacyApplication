
namespace TreeViewCrud.Models;
public class Unit
{
    private int _unitId;
    private string _name = null!;
    private string? _shortName;

    public int UnitId
    {
        get => _unitId;
        set => _unitId = value;
    }

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Name cannot be empty");
            _name = value;
        }
    }

    public string? ShortName
    {
        get => _shortName;
        set => _shortName = value;
    }

    public Unit() { }

    public Unit(string name, string? shortName = null)
    {
        Name = name;
        ShortName = shortName;
    }
}