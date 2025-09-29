using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rankings.Models;

public sealed class RankingArgs(string date)
{
    public DateTime Date { get; set; } = ValidationServices.ValidateDateTime(date);
}
