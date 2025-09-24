namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date, int capacity)
{
    public string Name { get; init; } = ValidateName(name);
    public DateOnly Date { get; init; } = ValidateEventDate(date);
    public int Capacity { get; set; } = capacity;

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid event name");
        }

        return name;
    }

    private static DateOnly ValidateEventDate(string date)
    {
        if (!DateOnly.TryParseExact(date, "yyyy-MM-dd", out var parsedDate))
        {
            throw new ArgumentException(
                $"Invalid date format: {date}. Expected format is yyyy-MM-dd");
        }

        if (parsedDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                $"Invalid event date: {parsedDate:yyyy-MM-dd}. Event date cannot be in the past");
        }

        return parsedDate;
    }
}
