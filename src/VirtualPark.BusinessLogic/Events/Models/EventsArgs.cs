namespace VirtualPark.BusinessLogic.Events.Models;

public sealed class EventsArgs
{
    public string Name { get; init; }

    public EventsArgs(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Invalid event name");
        }

        Name = name;
    }
}
