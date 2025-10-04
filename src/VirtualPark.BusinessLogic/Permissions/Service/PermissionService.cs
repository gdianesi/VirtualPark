using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Permissions.Models;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Permissions.Service;

public sealed class PermissionService(IRepository<Role> roleRepository, IRepository<Permission> permissionRepository)
{
    private readonly IRepository<Permission> _permissionRepository = permissionRepository;
    private readonly IRepository<Role> _roleRepository = roleRepository;

    public Guid Create(PermissionArgs args)
    {
        List<Role> roles = ValidateAndLoadRoles(args.Roles);
        Permission entity = MapToEntity(args, roles);
        _permissionRepository.Add(entity);
        return entity.Id;
    }

    private static Permission MapToEntity(PermissionArgs args, List<Role> roles)
    {
        return new Permission { Key = args.Key, Description = args.Description, Roles = roles };
    }

    private List<Role> ValidateAndLoadRoles(List<Guid> rolesIds)
    {
        var attractions = new List<Role>();

        foreach(Guid id in rolesIds)
        {
            Role? attraction = _roleRepository.Get(a => a.Id == id);
            if(attraction == null)
            {
                throw new InvalidOperationException($"Role with id {id} not found.");
            }

            attractions.Add(attraction);
        }

        return attractions;
    }

    public void Update(Guid id, PermissionArgs args)
    {
        Permission? permission = _permissionRepository.Get(p => p.Id == id);
        if(permission != null)
        {
            List<Role> roles = ValidateAndLoadRoles(args.Roles);
            ApplyArgsToEntity(permission, args, roles);
            _permissionRepository.Update(permission);
        }
    }

    private static void ApplyArgsToEntity(Permission entity, PermissionArgs args, List<Role> roles)
    {
        entity.Key = args.Key;
        entity.Description = args.Description;
        entity.Roles = roles;
    }

    public void Remove(Guid id)
    {
        var permission = _permissionRepository.Get(p => p.Id == id)
                         ?? throw new InvalidOperationException($"Permission with id {id} not found.");

        _permissionRepository.Remove(permission);
    }

    public List<Permission> GetAll()
    {
        return _permissionRepository.GetAll().ToList();
    }

    public Permission? GetById(Guid id)
    {
        return _permissionRepository.Get(p => p.Id == id);
    }
}
