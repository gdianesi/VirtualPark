using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Models;

public class VisitorProfileArgs
{
    public DateOnly DateOfBirth { get; init; }
    public Membership Membership { get; init; }

    public VisitorProfileArgs(string dateOfBirth, string membership)
    {
        DateOfBirth = ParseDateOfBirth(dateOfBirth);

        Membership = ParseMembership(membership);
    }

    private static DateOnly ParseDateOfBirth(string dateOfBirth)
    {
        var isNotValid = !DateOnly.TryParseExact(dateOfBirth, "yyyy-MM-dd", out var parsedDate);
        if (isNotValid)
        {
            throw new ArgumentException(
                $"Invalid date format: {dateOfBirth}. Expected format is yyyy-MM-dd",
                nameof(dateOfBirth)
            );
        }

        return parsedDate;
    }

    private static Membership ParseMembership(string membership)
    {
        var isNotValid = !Enum.TryParse<Membership>(membership, true, out var parsedMembership);
        if (isNotValid)
        {
            throw new ArgumentException(
                $"Invalid membership value: {membership}",
                nameof(membership)
            );
        }

        return parsedMembership;
    }
}
