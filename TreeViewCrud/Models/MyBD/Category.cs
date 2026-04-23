
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TreeViewCrud.Models;
[Table("Category")]
public class Category : EntityBase
{
    private int _categoryId;
    private string _name = null!;
    private string? _description;
    public virtual ObservableCollection<Item> Items { get; set; } = new();
    [Key]
    [Column("CategoryId")]
    public int CategoryId
    {
        get => _categoryId;
        set => _categoryId = value;
    }
    [Column("Name")]
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
    [Column("Description")]
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