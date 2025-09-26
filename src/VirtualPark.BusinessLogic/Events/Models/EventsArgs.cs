using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity, int cost)
{
    public string Name { get; init; } = ValidationServices.ValidateNullOrEmpty(name);
    public DateOnly Date { get; init; } = ValidationServices.ValidateDate(date);
    public int Capacity { get; set; } = ValidatePositive(capacity);
    public int Cost { get; set; } = ValidatePositive(cost);

    private static int ValidatePositive(int number)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(number);

        return number;
    }
}
