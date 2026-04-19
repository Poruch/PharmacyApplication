namespace TreeViewCrud.Models;


public class Supplier
{
    private int _supplierId;
    private string _companyName = null!;
    private string? _inn;
    private string? _phone;
    private string? _email;

    public int SupplierId
    {
        get => _supplierId;
        set => _supplierId = value;
    }

    public string CompanyName
    {
        get => _companyName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Company name cannot be empty");
            _companyName = value;
        }
    }

    public string? Inn
    {
        get => _inn;
        set => _inn = value;
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

    public Supplier() { }

    public Supplier(string companyName, string? inn = null, string? phone = null, string? email = null)
    {
        CompanyName = companyName;
        Inn = inn;
        Phone = phone;
        Email = email;
    }
}