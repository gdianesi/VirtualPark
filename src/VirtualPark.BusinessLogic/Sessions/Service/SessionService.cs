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
}
