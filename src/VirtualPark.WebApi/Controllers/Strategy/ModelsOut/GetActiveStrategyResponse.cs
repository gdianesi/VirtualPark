namespace VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

public class GetActiveStrategyResponse(string key, string date)
{
    public string Key { get; } = key;
    public string Date { get; } = date;
}
