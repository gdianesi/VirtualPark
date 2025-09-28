using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.VisitorsProfile.Models;

public class VisitorProfileArgs(string dateOfBirth, string membership, string score)
{
    public DateOnly DateOfBirth { get; init; } = ValidationServices.ParseDateOfBirth(dateOfBirth);
    public Membership Membership { get; init; } = ValidationServices.ParseMembership(membership);
    public int Score { get; init; } = ValidationServices.ValidateAndParseInt(score);
}
