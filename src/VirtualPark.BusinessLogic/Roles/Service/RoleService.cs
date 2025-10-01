using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Roles.Service;

public sealed class RoleService(IReadOnlyRepository<Permission> permissionReadOnlyRepositor)
{
    private readonly IReadOnlyRepository<Permission> _permissionReadOnlyRepositor = permissionReadOnlyRepositor;
}
