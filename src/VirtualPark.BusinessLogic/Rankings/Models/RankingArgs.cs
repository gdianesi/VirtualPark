using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Rankings.Models;

public sealed class RankingArgs(string date, string[] entries)
{
    public DateTime Date { get; init; } = ValidationServices.ValidateDateTime(date);
    public List<Guid> Entries { get; init; } = entries.Select(ValidationServices.ValidateAndParseGuid).ToList();
}
