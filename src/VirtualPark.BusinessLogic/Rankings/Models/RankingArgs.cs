using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rankings.Models;

public sealed class RankingArgs(string date, string period)
{
    public DateTime Date { get; } = ValidationServices.ValidateDateTime(date);
    public Period Period { get; } = ValidationServices.ValidateAndParsePeriod(period);
}
