namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date)
{
    public string Name { get; init; } = ValidateName(name);
    public DateOnly Date { get; init; } = ValidateDate(date);

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid event name");
        }

        return name;
    }

    private static DateOnly ValidateDate(string date)
    {
        var isNotValid = !DateOnly.TryParseExact(date, "yyyy-MM-dd", out var parsedDate);

        if (isNotValid)
        {
            throw new ArgumentException("Invalid event date format. Expected yyyy-MM-dd");
        }

        return parsedDate;
    }
}
