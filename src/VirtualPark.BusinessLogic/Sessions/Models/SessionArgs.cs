using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Sessions.Models;

public class SessionArgs(string email, string password)
{
    public string Email { get; } = ValidationServices.ValidateEmail(email);
    public string Password { get; } = ValidationServices.ValidatePassword(password);
}
