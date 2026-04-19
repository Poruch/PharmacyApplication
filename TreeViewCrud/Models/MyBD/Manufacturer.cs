namespace TreeViewCrud.Models;
public class Manufacturer
{
    private int _manufacturerId;
    private string _name = null!;
    private string? _country;
    private string? _phone;
    private string? _email;

    public int ManufacturerId
    {
        get => _manufacturerId;
        set => _manufacturerId = value;
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

    public string? Country
    {
        get => _country;
        set => _country = value;
    }

    public string? Phone
    {
        get => _phone;
        set => _phone = value;
    }

    public string? Email
    {
        get => _email;
        set => _email = value;
    }

    public Manufacturer() { }

    public Manufacturer(string name, string? country = null, string? phone = null, string? email = null)
    {
        Name = name;
        Country = country;
        Phone = phone;
        Email = email;
    }
}