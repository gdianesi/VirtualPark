using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Users.Entity;

namespace VirtualPark.BusinessLogic.Sessions.Service;

public interface ISessionService
{
    public Guid LogIn(SessionArgs args);
    public User GetUserLogged(Guid token);
    public void LogOut(Guid token);
}
