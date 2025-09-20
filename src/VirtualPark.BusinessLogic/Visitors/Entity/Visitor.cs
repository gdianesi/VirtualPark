namespace VirtualPark.BusinessLogic.Visitors.Entity;

public sealed class Visitor
{
    public Guid Id { get; } = Guid.NewGuid();

    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set => _dateOfBirth = ValidateDateOfBirth(value);
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set => _name = ValidateName(value);
    }

    public Visitor(string name)
    {
        Name = name;
    }

    private static DateTime ValidateDateOfBirth(DateTime date)
    {
        if (date > DateTime.UtcNow)
        {
            throw new ArgumentException("Date of birth cannot be in the future", nameof(date));
        }

        return date;
    }

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty", nameof(name));
        }

        return name;
    }
}
