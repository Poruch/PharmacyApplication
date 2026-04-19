
namespace TreeViewCrud.Models;
public class Sale
{
    private int _saleId;
    private DateTime _saleDate;
    private decimal _totalAmount;
    private string _paymentType = null!;
    private int _userId;

    public int SaleId
    {
        get => _saleId;
        set => _saleId = value;
    }

    public DateTime SaleDate
    {
        get => _saleDate;
        set => _saleDate = value;
    }

    public decimal TotalAmount
    {
        get => _totalAmount;
        set
        {
            if (value < 0)
                throw new ArgumentException("Total amount cannot be negative");
            _totalAmount = value;
        }
    }

    public string PaymentType
    {
        get => _paymentType;
        set
        {
            var allowed = new[] { "cash", "card" };
            if (!allowed.Contains(value))
                throw new ArgumentException($"PaymentType must be one of: {string.Join(", ", allowed)}");
            _paymentType = value;
        }
    }

    public int UserId
    {
        get => _userId;
        set
        {
            if (value <= 0)
                throw new ArgumentException("UserId must be positive");
            _userId = value;
        }
    }

    public Sale() { }

    public Sale(decimal totalAmount, string paymentType, int userId, DateTime? saleDate = null)
    {
        TotalAmount = totalAmount;
        PaymentType = paymentType;
        UserId = userId;
        SaleDate = saleDate ?? DateTime.Now;
    }
}