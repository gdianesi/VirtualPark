using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitsScore.Models;

namespace VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;

public class VisitScoreRequest
{
    public string? VisitRegistrationId { get; init; }
    public string? Origin { get; init; }
    public string? Points { get; init; }
    public RecordVisitScoreArgs ToArgs()
    {
        return new RecordVisitScoreArgs(
            ValidationServices.ValidateNullOrEmpty(VisitRegistrationId),
            ValidationServices.ValidateNullOrEmpty(Origin),
            Points);
    }
}
