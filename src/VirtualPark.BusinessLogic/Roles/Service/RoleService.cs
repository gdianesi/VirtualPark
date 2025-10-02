using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Roles.Service;

public sealed class RoleService(IRepository<Role> roleRepository, IReadOnlyRepository<Permission> permissionReadOnlyRepositor)
{
    private readonly IRepository<Role> _roleRepostiory = roleRepository;
    private readonly IReadOnlyRepository<Permission> _permissionReadOnlyRepositor = permissionReadOnlyRepositor;

    public void ApplyArgsToEntity(Role role, RoleArgs args)
    {
        role.Name = args.Name;
        role.Description = args.Description;
        role.Permissions = GuidToPermission(args.PermissionIds);
    }

    public void ValidateRoleName(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Role name cannot be empty.", nameof(name));
        }

        if(_roleRepostiory.Exist(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
        {
            throw new Exception("Role name already exists.");
        }
    }

    public List<Permission> GuidToPermission(List<Guid> permissionIds)
    {
        ArgumentNullException.ThrowIfNull(permissionIds);

        return permissionIds.Select(guid =>
            _permissionReadOnlyRepositor.Get(p => p.Id == guid) ??
            throw new KeyNotFoundException($"Permission with id {guid} does not exist")).ToList();
    }

    public Role MapToEntity(RoleArgs roleArgs)
    {
        ArgumentNullException.ThrowIfNull(roleArgs);

        return new Role
        {
            Name = roleArgs.Name,
            Description = roleArgs.Description,
            Permissions = GuidToPermission(roleArgs.PermissionIds)
        };
    }
}
