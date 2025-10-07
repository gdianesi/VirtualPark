using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Users.Service;

public class UserService(IRepository<User> userRepository, IReadOnlyRepository<Role> rolesRepository, IVisitorProfileService visitorProfileServiceService,
    IReadOnlyRepository<VisitorProfile> visitorProfileRepository) : IUserService
{
    private readonly IRepository<User> _userRepository = userRepository;
    private readonly IReadOnlyRepository<Role> _rolesRepository = rolesRepository;
    private readonly IVisitorProfileService _visitorProfileServiceService = visitorProfileServiceService;
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

        if(user == null)
        {
            throw new InvalidOperationException("User don't exist");
        }

        if(user.VisitorProfileId != null)
        {
            user.VisitorProfile =
                _visitorProfileRepository.Get(visitorProfile => visitorProfile.Id == user.VisitorProfileId);
        }

        return user;
    }

    public List<User> GetAll()
    {
        var users = _userRepository.GetAll();

        if(users == null)
        {
            throw new InvalidOperationException("Dont have any users");
        }

        UploadVisitorProfile(users);
        return users;
    }

    public void Remove(Guid id)
    {
        var user = _userRepository.Get(u => u.Id == id);

        if(user == null)
        {
            throw new InvalidOperationException("User don't exist");
        }

        if(user.VisitorProfileId != null)
        {
            DeleteVisitorProfile(user.VisitorProfileId);
        }

        _userRepository.Remove(user);
    }

    public void Update(UserArgs args, Guid userId)
    {
        var user = Get(userId);

        ApplyChange(user!, args);

        _userRepository.Update(user!);
    }

    private void ApplyChange(User user, UserArgs args)
    {
        user.Name = args.Name;
        user.LastName = args.LastName;
        user.Password = args.Password;
        if(args.VisitorProfile != null && user.VisitorProfileId != null)
        {
            _visitorProfileServiceService.Update(args.VisitorProfile, (Guid)user.VisitorProfileId);
        }
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
        Roles = GetRolesFromIds(args),
        VisitorProfile = visitorProfile,
        VisitorProfileId = visitorProfile?.Id,
    };

    private VisitorProfile? CreateVisitorProfile(VisitorProfileArgs? visitorArgs)
    {
        if(visitorArgs != null)
        {
            return _visitorProfileServiceService.Create(visitorArgs);
        }

        return null;
    }

    private List<Role> GetRolesFromIds(UserArgs userArgs)
    {
        var roles = new List<Role>();
        List<Guid> roleIds = userArgs.RolesIds;

        foreach(var roleId in roleIds)
        {
            var role = _rolesRepository.Get(r => r.Id == roleId);
            if(role is null)
            {
                throw new InvalidOperationException($"Role with id {roleId} does not exist.");
            }

            if(role.Name == "Visitor" && userArgs.VisitorProfile == null)
            {
                throw new InvalidOperationException($"You have a visitor role but you don't have a visitor profile.");
            }

            roles.Add(role);
        }

        return roles;
    }

    private List<User> UploadVisitorProfile(List<User> users)
    {
        foreach(var u in users)
        {
            if(u.VisitorProfileId.HasValue)
            {
                u.VisitorProfile =
                    _visitorProfileRepository.Get(visitorProfile => visitorProfile.Id == u.VisitorProfileId);
            }
        }

        return users;
    }

    private void DeleteVisitorProfile(Guid? id)
    {
        _visitorProfileServiceService.Remove(id);
    }
}
