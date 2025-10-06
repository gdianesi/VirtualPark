using VirtualPark.BusinessLogic.Sessions.Entity;
using VirtualPark.BusinessLogic.Sessions.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Sessions.Service;

public class SessionService(IRepository<Session> sessionRepository, IReadOnlyRepository<User> userRepository)
{
    private readonly IRepository<Session> _sessionRepository = sessionRepository;
    private readonly IReadOnlyRepository<User> _userRepository = userRepository;

    public Guid LogIn(SessionArgs args)
    {
        var session = MapToEntity(args);

        _sessionRepository.Add(session);

        return session.Id;
    }

    public User GetUserLogged(Guid token)
    {
        var session = GetSession(token);

        var user = GetUser(session.UserId);

        return user;
    }

    public void LogOut(Guid token)
    {
        var session = GetSession(token);

        _sessionRepository.Remove(session);
    }

    private Session MapToEntity(SessionArgs args) => new Session
    {
        UserId = args.UserId,
        User = GetUser(args.UserId),
    };

    private User GetUser(Guid userId)
    {
        var user = _userRepository.Get(u => u.Id == userId);

        if(user is null)
        {
            throw new InvalidOperationException($"User not exist.");
        }

        return user;
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
