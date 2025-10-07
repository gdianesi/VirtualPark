namespace VirtualPark.WebApi.Controllers.Sessions.ModelsOut;

public class LogInSessionResponse(string token)
{
    public string Token { get; } = token;
}
