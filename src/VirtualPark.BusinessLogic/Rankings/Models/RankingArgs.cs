using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rankings.Models;

public sealed class RankingArgs(string date, string period)
{
    public DateTime Date { get; init; } = ValidationServices.ValidateDateTime(date);
    public Period Period { get; init; } = ValidationServices.ValidateAndParsePeriod(period);
}
