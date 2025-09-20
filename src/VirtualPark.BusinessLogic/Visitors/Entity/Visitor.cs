using System.Text.RegularExpressions;

namespace VirtualPark.BusinessLogic.Visitors.Entity;

public sealed class Visitor
{
    private DateTime _dateOfBirth;

    private string _email = string.Empty;

    private string _lastName = string.Empty;

    private string _name = string.Empty;

    private string _passwordHash = string.Empty;

    public Visitor(string name, string lastName, DateTime db, string email, string passwordHash)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Score = 0;
        Membership = Membership.Standard;
        DateOfBirth = db;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string PasswordHash
    {
        get => _passwordHash;
        private set => _passwordHash = ValidatePassword(value);
    }

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = ValidateDateOfBirth(value);
    }

    public string Name
    {
        get => _name;
        set => _name = ValidateName(value);
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = ValidateLastName(value);
    }

    public string Email
    {
        get => _email;
        set => _email = ValidateEmail(value);
    }

    public int Score { get; private set; }
    public Membership Membership { get; private set; }

    private static DateTime ValidateDateOfBirth(DateTime date)
    {
        if(date > DateTime.UtcNow)
        {
            throw new ArgumentException("Date of birth cannot be in the future");
        }

        return date;
    }

    private static string ValidateName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty");
        }

        return name;
    }

    private static string ValidateLastName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Last name cannot be null or empty");
        }

        return name;
    }

    private static string ValidateEmail(string email)
    {
        if(string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email format is invalid");
        }

        var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        if(!regex.IsMatch(email))
        {
            throw new ArgumentException("Email format is invalid");
        }

        return email;
    }

    private static string ValidatePassword(string passwordHash)
    {
        if(string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("The password hash cannot be null or empty");
        }

        return passwordHash;
    }
}
