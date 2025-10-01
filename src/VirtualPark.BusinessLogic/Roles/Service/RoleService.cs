using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Roles.Service;

public sealed class RoleService(IReadOnlyRepository<Permission> permissionReadOnlyRepositor)
{
    private readonly IReadOnlyRepository<Permission> _permissionReadOnlyRepositor = permissionReadOnlyRepositor;

    public List<Permission> GuidToPermission(List<Guid> permissionIds)
    {
        ArgumentNullException.ThrowIfNull(permissionIds);

        return permissionIds.Select(guid =>
            _permissionReadOnlyRepositor.Get(p => p.Id == guid) ??
            throw new KeyNotFoundException($"Permission with id {guid} does not exist")).ToList();
    }
}
