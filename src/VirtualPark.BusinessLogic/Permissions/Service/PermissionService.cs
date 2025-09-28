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
        var entity = new Permission
        {
            Description = args.Description,
            Key = args.Key
        };

        foreach (Guid id in args.Roles)
        {
            Role? role = _roleRepository.Get(r => r.Id == id);
            if (role == null)
            {
                throw new InvalidOperationException($"Role with id {id} not found.");
            }

            entity.Roles.Add(role);
        }

        _permissionRepository.Add(entity);

        return entity.Id;
    }
}
