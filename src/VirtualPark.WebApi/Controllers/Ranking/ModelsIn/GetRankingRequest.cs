using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Ranking.ModelsIn;

public sealed class GetRankingRequest
{
    public string Date { get; init; } = string.Empty;
    public string Period { get; init; } = string.Empty;

    public RankingArgs ToArgs()
    {
        var validatedDate = ValidationServices.ValidateNullOrEmpty(Date);
        var validatedPeriod = ValidationServices.ValidateNullOrEmpty(Period);

        return new RankingArgs(validatedDate, validatedPeriod);
    }
}
