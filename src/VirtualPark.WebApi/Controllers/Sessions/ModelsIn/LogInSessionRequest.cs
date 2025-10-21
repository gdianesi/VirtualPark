using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.WebApi.Controllers.Sessions.ModelsIn;

public class LogInSessionRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public SessionArgs ToArgs()
    {
        return new SessionArgs(
            ValidationServices.ValidateNullOrEmpty(Email),
            ValidationServices.ValidateNullOrEmpty(Password));
    }
}
