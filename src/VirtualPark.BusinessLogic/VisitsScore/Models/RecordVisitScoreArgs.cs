using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.VisitsScore.Models;

public sealed class RecordVisitScoreArgs(string visitRegistrationId, string origin, string? points = null)
{
    public Guid VisitRegistrationId { get; } = ValidationServices.ValidateAndParseGuid(visitRegistrationId);
    public string Origin { get; } = ValidationServices.ValidateNullOrEmpty(origin).Trim();
    public int? Points { get; } = string.IsNullOrWhiteSpace(points) ? null : ValidationServices.ValidateAndParseInt(points);
}
