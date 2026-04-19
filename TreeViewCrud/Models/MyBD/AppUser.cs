
namespace TreeViewCrud.Models;
public class AppUser
{
    private int _userId;
    private string _lastName = null!;
    private string _firstName = null!;
    private string? _patronymic;
    private string _login = null!;
    private string _passwordHash = null!;
    private string _role = null!;

    public int UserId
    {
        get => _userId;
        set => _userId = value;
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Last name cannot be empty");
            _lastName = value;
        }
    }

    public string FirstName
    {
        get => _firstName;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name cannot be empty");
            _firstName = value;
        }
    }

    public string? Patronymic
    {
        get => _patronymic;
        set => _patronymic = value;
    }

    public string Login
    {
        get => _login;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Login cannot be empty");
            _login = value;
        }
    }

    public string PasswordHash
    {
        get => _passwordHash;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Password hash cannot be empty");
            _passwordHash = value;
        }
    }

    public string Role
    {
        get => _role;
        set
        {
            var allowed = new[] { "provizor", "pharmacist", "manager", "admin" };
            if (!allowed.Contains(value))
                throw new ArgumentException($"Role must be one of: {string.Join(", ", allowed)}");
            _role = value;
        }
    }

    public AppUser() { }

    public AppUser(string lastName, string firstName, string login, string passwordHash, string role,
                   string? patronymic = null)
    {
        LastName = lastName;
        FirstName = firstName;
        Patronymic = patronymic;
        Login = login;
        PasswordHash = passwordHash;
        Role = role;
    }
}