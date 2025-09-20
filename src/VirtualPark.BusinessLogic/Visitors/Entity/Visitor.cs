namespace VirtualPark.BusinessLogic.Visitors.Entity;

public sealed class Visitor
{
    public Guid Id { get; init; } = Guid.NewGuid();

    private DateTime _dateOfBirth;

    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if(value > DateTime.UtcNow)
            {
                throw new ArgumentException("Date of birth cannot be in the future");
            }

            _dateOfBirth = value;
        }
    }

    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be null or empty");
            }

            _name = value;
        }
    }

    public Visitor(string name)
    {
        Name = name;
    }
}
