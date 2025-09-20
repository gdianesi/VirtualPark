namespace VirtualPark.BusinessLogic.Visitors.Entity;

public sealed class Visitor
{
    public Guid Id { get; init; } = Guid.NewGuid();
    private DateTime _dateOfBirth;
    public DateTime DateOfBirth
    {
        get => _dateOfBirth;
        init
        {
            if (value > DateTime.UtcNow)
            {
                throw new ArgumentException("Date of birth cannot be in the future");
            }

            _dateOfBirth = value;
        }
    }
}
