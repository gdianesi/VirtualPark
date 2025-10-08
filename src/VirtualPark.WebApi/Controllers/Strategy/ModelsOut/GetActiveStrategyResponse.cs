namespace VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

public class GetActiveStrategyResponse(string id, string key, string date)
{
    public string Id { get; } = id;
    public string Key { get; } = key;
    public string Date { get; } = date;
}
