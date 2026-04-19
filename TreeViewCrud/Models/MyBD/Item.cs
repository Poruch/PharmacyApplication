
namespace TreeViewCrud.Models;
public class Item : EntityBase
{
    private int _itemId;
    private string _name = null!;
    private string? _mnfName;
    private string? _dosage;
    private string? _form;
    private bool _prescriptionRequired;
    private int _categoryId;
    private int _manufacturerId;
    private int _unitId;

    public int ItemId
    {
        get => _itemId;
        set => _itemId = value;
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

    public string? MnfName
    {
        get => _mnfName;
        set => _mnfName = value;
    }

    public string? Dosage
    {
        get => _dosage;
        set => _dosage = value;
    }

    public string? Form
    {
        get => _form;
        set => _form = value;
    }

    public bool PrescriptionRequired
    {
        get => _prescriptionRequired;
        set => _prescriptionRequired = value;
    }

    public int CategoryId
    {
        get => _categoryId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("CategoryId must be positive");
            _categoryId = value;
        }
    }

    public int ManufacturerId
    {
        get => _manufacturerId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("ManufacturerId must be positive");
            _manufacturerId = value;
        }
    }

    public int UnitId
    {
        get => _unitId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("UnitId must be positive");
            _unitId = value;
        }
    }

    public Item() { }

    public Item(string name, int categoryId, int manufacturerId, int unitId, bool prescriptionRequired = false,
                string? mnfName = null, string? dosage = null, string? form = null)
    {
        Name = name;
        CategoryId = categoryId;
        ManufacturerId = manufacturerId;
        UnitId = unitId;
        PrescriptionRequired = prescriptionRequired;
        MnfName = mnfName;
        Dosage = dosage;
        Form = form;
    }
}