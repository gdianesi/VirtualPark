using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Sessions.Models;

public class SessionArgs(string email, string password)
{
    public string Email { get; init; } = ValidationServices.ValidateEmail(email);
    public string Password { get; init; } = ValidationServices.ValidatePassword(password);
}
