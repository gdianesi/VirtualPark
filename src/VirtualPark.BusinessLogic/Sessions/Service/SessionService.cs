using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Sessions.Service;

public class SessionService(IRepository<Session> sessionRepository, IReadOnlyRepository<User> userRepository) : ISessionService
{
    private readonly IRepository<Session> _sessionRepository = sessionRepository;
    private readonly IReadOnlyRepository<User> _userRepository = userRepository;

    public Guid LogIn(SessionArgs args)
    {
        ValidationUser(args);

        var session = MapToEntity(args);

        _sessionRepository.Add(session);

        return session.Token;
    }

    public User GetUserLogged(Guid token)
    {
        var session = GetSession(token);

        var user = GetUser(session.Email);

        return user;
    }

    public void LogOut(Guid token)
    {
        var session = GetSession(token);

        _sessionRepository.Remove(session);
    }

    private Session MapToEntity(SessionArgs args) => new Session
    {
        Email = args.Email,
        Password = args.Password,
        User = GetUser(args.Email),
        UserId = GetUser(args.Email).Id
    };

    private User GetUser(string email)
    {
        var user = _userRepository.Get(u => u.Email == email);

        if(user is null)
        {
            throw new InvalidOperationException($"Invalid credentials.");
        }

        return user;
    }

    private void ValidationUser(SessionArgs args)
    {
        var user = GetUser(args.Email);

        if(user.Password != args.Password)
        {
            throw new InvalidOperationException($"Invalid credentials.");
        }
    }

    private Session GetSession(Guid token)
    {
        var session = _sessionRepository.Get(r => r.Token == token);

        if(session is null)
        {
            throw new Exception("Session not found or the token has expired.");
        }

        return session;
    }
}
