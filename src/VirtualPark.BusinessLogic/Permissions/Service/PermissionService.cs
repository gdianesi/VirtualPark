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
        var existing = _permissionRepository.Get(p => p.Id == id);
        if (existing == null)
        {
            throw new InvalidOperationException($"Permission with id {id} not found.");
        }

        foreach (var roleId in from roleId in args.Roles let role = _roleRepository.Get(r => r.Id == roleId) where role == null select roleId)
        {
            throw new InvalidOperationException($"Role with id {roleId} not found.");
        }
    }
}
