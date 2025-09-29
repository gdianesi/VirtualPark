using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rankings.Models;

public sealed class RankingArgs(string date)
{
    public DateTime Date { get; init; } = ValidationServices.ValidateDateTime(date);
}
