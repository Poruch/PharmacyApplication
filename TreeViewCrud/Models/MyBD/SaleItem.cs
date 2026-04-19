
namespace TreeViewCrud.Models;
public class SaleItem
{
    private int _saleItemId;
    private int _quantity;
    private decimal _priceAtSale;
    private int _saleId;
    private int? _uniqueItemId;
    private int _itemId;

    public int SaleItemId
    {
        get => _saleItemId;
        set => _saleItemId = value;
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new ArgumentException("Quantity must be positive");
            _quantity = value;
        }
    }

    public decimal PriceAtSale
    {
        get => _priceAtSale;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price cannot be negative");
            _priceAtSale = value;
        }
    }

    public int SaleId
    {
        get => _saleId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("SaleId must be positive");
            _saleId = value;
        }
    }

    public int? UniqueItemId
    {
        get => _uniqueItemId;
        set => _uniqueItemId = value;
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

    public SaleItem() { }

    public SaleItem(int quantity, decimal priceAtSale, int saleId, int itemId, int? uniqueItemId = null)
    {
        Quantity = quantity;
        PriceAtSale = priceAtSale;
        SaleId = saleId;
        ItemId = itemId;
        UniqueItemId = uniqueItemId;
    }
}
