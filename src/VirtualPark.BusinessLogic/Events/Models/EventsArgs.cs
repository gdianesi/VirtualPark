using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity, int cost, List<Guid> attractions)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public DateOnly Date { get; init; } = ValidationServices.ValidateDate(date);
    public int Capacity { get; set; } = ValidatePositive(capacity);
    public int Cost { get; set; } = ValidatePositive(cost);
    public List<Guid> AttractionIds { get; set; } = ValidateGuids(attractions);

    private static int ValidatePositive(int number)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);

        return number;
    }

    private static List<Guid> ValidateGuids(List<Guid> ids)
    {
        if(ids == null || ids.Count == 0)
        {
            throw new ArgumentException("Attractions list cannot be null or empty");
        }

        if(ids.Any(id => id == Guid.Empty))
        {
            throw new ArgumentException("Attractions list contains invalid Guid");
        }

        return ids;
    }
}
