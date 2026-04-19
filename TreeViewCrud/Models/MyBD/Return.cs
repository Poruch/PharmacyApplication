

namespace TreeViewCrud.Models;
public class Return
{
    private int _returnId;
    private DateTime _returnDate;
    private string? _reason;
    private decimal _refundAmount;
    private int _saleId;
    private int _userId;

    public int ReturnId
    {
        get => _returnId;
        set => _returnId = value;
    }

    public DateTime ReturnDate
    {
        get => _returnDate;
        set => _returnDate = value;
    }

    public string? Reason
    {
        get => _reason;
        set => _reason = value;
    }

    public decimal RefundAmount
    {
        get => _refundAmount;
        set
        {
            if (value < 0)
                throw new ArgumentException("Refund amount cannot be negative");
            _refundAmount = value;
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

    public Return() { }

    public Return(decimal refundAmount, int saleId, int userId, string? reason = null, DateTime? returnDate = null)
    {
        RefundAmount = refundAmount;
        SaleId = saleId;
        UserId = userId;
        Reason = reason;
        ReturnDate = returnDate ?? DateTime.Now;
    }
}