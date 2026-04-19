namespace TreeViewCrud.Models;
public class ReturnItem
{
    private int _returnItemId;
    private int _quantity;
    private int _returnId;
    private int? _uniqueItemId;
    private int _itemId;

    public int ReturnItemId
    {
        get => _returnItemId;
        set => _returnItemId = value;
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

    public int ReturnId
    {
        get => _returnId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("ReturnId must be positive");
            _returnId = value;
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

    public ReturnItem() { }

    public ReturnItem(int quantity, int returnId, int itemId, int? uniqueItemId = null)
    {
        Quantity = quantity;
        ReturnId = returnId;
        ItemId = itemId;
        UniqueItemId = uniqueItemId;
    }
}
