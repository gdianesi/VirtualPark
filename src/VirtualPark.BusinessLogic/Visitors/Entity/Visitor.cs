using System.ComponentModel.DataAnnotations;

namespace VirtualPark.BusinessLogic.Visitors.Entity;

public sealed class Visitor
{
    private DateTime _dateOfBirth;

    private string _email = string.Empty;

    private string _name = string.Empty;

    private string _passwordHash = string.Empty;

    public Visitor(string name, string email, string passwordHash)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
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

    public string Email
    {
        get => _email;
        set => _email = ValidateEmail(value);
    }

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

    private static string ValidateEmail(string email)
    {
        var validator = new EmailAddressAttribute();
        if(!validator.IsValid(email))
        {
            throw new ArgumentException("Email format is invalid");
        }

        return email;
    }

    private static string ValidatePassword(string passwordHash)
    {
        if(string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be null or empty", nameof(passwordHash));
        }

        return passwordHash;
    }
}
