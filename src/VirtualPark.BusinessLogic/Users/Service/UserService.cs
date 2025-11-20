using Microsoft.EntityFrameworkCore;
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
        var user = _userRepository.Get(
            u => u.Id == id,
            include: q => q.Include(u => u.Roles)) ?? throw new InvalidOperationException("User doesn't exist");
        if(user.VisitorProfileId != null)
        {
            user.VisitorProfile =
                _visitorProfileRepository.Get(vp => vp.Id == user.VisitorProfileId);
        }

        return user;
    }

    public List<User> GetAll()
    {
        var users = _userRepository.GetAll(
            predicate: null,
            include: q => q
                .Include(u => u.Roles)
                .Include(u => u.VisitorProfile));

        if(users == null || users.Count == 0)
        {
            throw new InvalidOperationException("Dont have any users");
        }

        return users;
    }

    public void Remove(Guid id)
    {
        var user = _userRepository.Get(
            u => u.Id == id,
            include: q => q.Include(u => u.Roles)) ?? throw new InvalidOperationException("User doesn't exist");
        if(user.Roles != null && user.Roles.Any())
        {
            user.Roles.Clear();
            _userRepository.Update(user);
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

    public bool HasPermission(Guid id, string permissionKey)
    {
        var user = _userRepository.Get(
            u => u.Id == id,
            include: q => q
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions));

        if(user?.Roles == null || user.Roles.Count == 0)
        {
            return false;
        }

        return user.Roles.Any(r =>
            r.Permissions != null &&
            r.Permissions.Any(p => p.Key == permissionKey));
    }

    private void ApplyChange(User user, UserArgs args)
    {
        user.Name = args.Name;
        user.LastName = args.LastName;
        user.Password = user.Password;
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

    private User MapToEntity(UserArgs args, VisitorProfile? visitorProfile)
    {
        if(string.IsNullOrWhiteSpace(args.Password))
        {
            throw new InvalidOperationException("Password is required for creating a user.");
        }

        return new User
        {
            Name = args.Name,
            LastName = args.LastName,
            Email = args.Email,
            Password = args.Password,
            Roles = GetRolesFromIds(args),
            VisitorProfile = visitorProfile,
            VisitorProfileId = visitorProfile?.Id,
        };
    }

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
            var role = _rolesRepository.Get(r => r.Id == roleId) ?? throw new InvalidOperationException($"Role with id {roleId} does not exist.");
            if(role.Name == "Visitor" && userArgs.VisitorProfile == null)
            {
                throw new InvalidOperationException($"You have a visitor role but you don't have a visitor profile.");
            }

            roles.Add(role);
        }

        return roles;
    }

    private void DeleteVisitorProfile(Guid? id)
    {
        _visitorProfileServiceService.Remove(id);
    }

    public User GetByVisitorProfileId(Guid visitorProfileId)
    {
        var user = _userRepository.Get(u => u.VisitorProfileId == visitorProfileId) ?? throw new InvalidOperationException("User for VisitorProfile not found");
        return user;
    }

    public List<User> GetByVisitorProfileIds(List<Guid> visitorProfileIds)
    {
        if(visitorProfileIds.Count == 0)
        {
            return [];
        }

        return _userRepository.GetAll(u => u.VisitorProfileId.HasValue &&
                                           visitorProfileIds.Contains(u.VisitorProfileId.Value));
    }
}
