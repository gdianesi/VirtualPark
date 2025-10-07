using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Permissions.Models;

namespace VirtualPark.BusinessLogic.Permissions.Service;

public interface IPermissionService
{
    public Guid Create(PermissionArgs args);
    public void Update(Guid id, PermissionArgs args);
    public void Remove(Guid id);
    public List<Permission> GetAll();
    public Permission? GetById(Guid id);
}
