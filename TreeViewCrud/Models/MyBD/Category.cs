
namespace TreeViewCrud.Models;
public class Category : EntityBase
{
    private int _categoryId;
    private string _name = null!;
    private string? _description;

    public int CategoryId
    {
        get => _categoryId;
        set => _categoryId = value;
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

    public string? Description
    {
        get => _description;
        set => _description = value;
    }

    public Category() { }

    public Category(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}