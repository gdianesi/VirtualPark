using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Users.Service;

public class UserService(IRepository<User> userRepository, IReadOnlyRepository<Role> rolesRepository, IVisitorProfile visitorProfileService,
    IReadOnlyRepository<VisitorProfile> visitorProfileRepository)
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IReadOnlyRepository<Role> _rolesRepository = rolesRepository;
    private readonly IVisitorProfile _visitorProfileService = visitorProfileService;
    private readonly IReadOnlyRepository<VisitorProfile> _visitorProfileRepository = visitorProfileRepository;

    public Guid Create(UserArgs args)
    {
        ValidateEmail(args);

        var visitorProfile = CreateVisitorProfile(args.VisitorProfile);

        var user = MapToEntity(args, visitorProfile);

        _userRepository.Add(user);
        return user.Id;
    }

    public User? Get(Guid id)
    {
        var user = _userRepository.Get(u => u.Id == id);

        user.VisitorProfile = _visitorProfileRepository.Get(visitorProfile => visitorProfile.Id == user.VisitorProfileId);
        return user;
    }

    private void ValidateEmail(UserArgs args)
    {
        var isEmailTaken = _userRepository.Exist(u => u.Email == args.Email);
        if(isEmailTaken)
        {
            throw new InvalidOperationException("Email already exists");
        }
    }

    private User MapToEntity(UserArgs args, VisitorProfile? visitorProfile) => new User
    {
        Name = args.Name,
        LastName = args.LastName,
        Email = args.Email,
        Password = args.Password,
        Roles = GetRolesFromIds(args.RolesIds),
        VisitorProfile = visitorProfile,
        VisitorProfileId = visitorProfile?.Id,
    };

    private VisitorProfile? CreateVisitorProfile(VisitorProfileArgs? visitorArgs)
    {
        if (visitorArgs != null)
        {
            return _visitorProfileService.Create(visitorArgs);
        }

        return null;
    }

    private List<Role> GetRolesFromIds(List<Guid> roleIds)
    {
        var roles = new List<Role>();

        foreach (var roleId in roleIds)
        {
            var role = _rolesRepository.Get(r => r.Id == roleId);
            if (role is null)
            {
                throw new InvalidOperationException($"Role with id {roleId} does not exist.");
            }

            roles.Add(role);
        }

        return roles;
    }
}
