namespace VirtualPark.BusinessLogic.VisitRegistrations.Modules;

public sealed class VisitRegistrationArgs(string date)
{
    public DateOnly Date { get; init; } = ValidateVisitDate(date);

    private static DateOnly ValidateVisitDate(string date)
    {
        if(!DateOnly.TryParseExact(date, "yyyy-MM-dd", out DateOnly parsedDate))
        {
            throw new ArgumentException(
                $"Invalid date format: {date}. Expected format is yyyy-MM-dd");
        }

        if(parsedDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                $"Invalid event date: {parsedDate:yyyy-MM-dd}. Event date cannot be in the past");
        }

        return parsedDate;
    }
}
