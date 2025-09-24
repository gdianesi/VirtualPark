using VirtualPark.BusinessLogic.Visitors.Entity;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Models;

public class VisitorProfileArgs
{
    public DateOnly DateOfBirth { get; init; }
    public Membership Membership { get; init; }

    public VisitorProfileArgs(string dateOfBirth, string membership)
    {
        if (!DateOnly.TryParseExact(dateOfBirth, "yyyy-MM-dd", out var parsedDate))
        {
            throw new ArgumentException(
                $"Invalid date format: {dateOfBirth}. Expected format is yyyy-MM-dd",
                nameof(dateOfBirth)
            );
        }

        DateOfBirth = parsedDate;
        if (!Enum.TryParse<Membership>(membership, true, out var parsedMembership))
        {
            throw new ArgumentException(
                $"Invalid membership value: {membership}",
                nameof(membership)
            );
        }

        Membership = parsedMembership;
    }
}
