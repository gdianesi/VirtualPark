namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs(string name, string date)
{
    public string Date { get; } = date;
    public string Name { get; init; } = ValidateName(name);

    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Invalid event name");
        }

        return name;
    }
}
