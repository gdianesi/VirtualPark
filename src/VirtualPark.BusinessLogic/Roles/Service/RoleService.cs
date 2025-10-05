using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Roles.Service;

public sealed class RoleService(IRepository<Role> roleRepository, IReadOnlyRepository<Permission> permissionReadOnlyRepositor)
{
    private readonly IRepository<Role> _roleRepository = roleRepository;
    private readonly IReadOnlyRepository<Permission> _permissionReadOnlyRepositor = permissionReadOnlyRepositor;

    public Guid Create(RoleArgs args)
    {
        ValidateRoleName(args.Name);

        Role role = MapToEntity(args);

        _roleRepository.Add(role);

        return role.Id;
    }

    public List<Role> GetAll()
    {
        return _roleRepository.GetAll();
    }

    public Role Get(Guid id)
    {
        var role = _roleRepository.Get(role => role.Id == id);

        if(role == null)
        {
            throw new InvalidOperationException("Role don't exist");
        }

        return role;
    }

    public void Update(RoleArgs args, Guid roleId)
    {
        var role = Get(roleId);

        ValidateRoleName(args.Name);

        ApplyArgsToEntity(role, args);
        _roleRepository.Update(role);
    }

    public void Remove(Guid id)
    {
        var role = Get(id);
        _roleRepository.Remove(role);
    }

    private void ApplyArgsToEntity(Role role, RoleArgs args)
    {
        role.Name = args.Name;
        role.Description = args.Description;
        role.Permissions = GuidToPermission(args.PermissionIds);
    }

    private void ValidateRoleName(string name)
    {
        if(_roleRepository.Exist(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)))
        {
            throw new Exception("Role name already exists.");
        }
    }

    private List<Permission> GuidToPermission(List<Guid> permissionIds)
    {
        return permissionIds.Select(guid =>
            _permissionReadOnlyRepositor.Get(p => p.Id == guid) ??
            throw new KeyNotFoundException($"Permission with id {guid} does not exist")).ToList();
    }

    private Role MapToEntity(RoleArgs roleArgs)
    {
        return new Role
        {
            Name = roleArgs.Name,
            Description = roleArgs.Description,
            Permissions = GuidToPermission(roleArgs.PermissionIds)
        };
    }
}
