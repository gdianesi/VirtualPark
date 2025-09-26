using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity, int cost)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public DateOnly Date { get; init; } = ValidationServices.ValidateDate(date);
    public int Capacity { get; set; } = ValidateEventCapacity(capacity);
    public int Cost { get; set; } = ValidateEventCost(cost);

    private static int ValidateEventCost(int cost)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(cost);

        return cost;
    }

    private static int ValidateEventCapacity(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);

        return capacity;
    }
}
