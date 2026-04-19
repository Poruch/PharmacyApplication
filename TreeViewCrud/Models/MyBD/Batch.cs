
namespace TreeViewCrud.Models;
public class Batch : EntityBase
{
    private int _batchId;
    private string _serialNumber = null!;
    private DateTime _productionDate;
    private DateTime? _expiryDate;
    private decimal _purchasePrice;
    private decimal _sellingPrice;
    private int _quantity;
    private int _itemId;
    private int _supplierId;
    private int _locationId;
    public virtual Item Item { get; set; }

    public int BatchId
    {
        get => _batchId;
        set => _batchId = value;
    }

    public string SerialNumber
    {
        get => _serialNumber;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Serial number cannot be empty");
            _serialNumber = value;
        }
    }

    public DateTime ProductionDate
    {
        get => _productionDate;
        set => _productionDate = value;
    }

    public DateTime? ExpiryDate
    {
        get => _expiryDate;
        set => _expiryDate = value;
    }

    public decimal PurchasePrice
    {
        get => _purchasePrice;
        set
        {
            if (value < 0)
                throw new ArgumentException("Purchase price cannot be negative");
            _purchasePrice = value;
        }
    }

    public decimal SellingPrice
    {
        get => _sellingPrice;
        set
        {
            if (value < 0)
                throw new ArgumentException("Selling price cannot be negative");
            _sellingPrice = value;
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value < 0)
                throw new ArgumentException("Quantity cannot be negative");
            _quantity = value;
        }
    }

    public int ItemId
    {
        get => _itemId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("ItemId must be positive");
            _itemId = value;
        }
    }

    public int SupplierId
    {
        get => _supplierId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("SupplierId must be positive");
            _supplierId = value;
        }
    }

    public int LocationId
    {
        get => _locationId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("LocationId must be positive");
            _locationId = value;
        }
    }

    public Batch() { }

    public Batch(string serialNumber, DateTime productionDate, decimal purchasePrice, decimal sellingPrice, int quantity,
                 int itemId, int supplierId, int locationId, DateTime? expiryDate = null)
    {
        SerialNumber = serialNumber;
        ProductionDate = productionDate;
        ExpiryDate = expiryDate;
        PurchasePrice = purchasePrice;
        SellingPrice = sellingPrice;
        Quantity = quantity;
        ItemId = itemId;
        SupplierId = supplierId;
        LocationId = locationId;
    }
}