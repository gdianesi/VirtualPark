using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Roles.Models;

namespace VirtualPark.BusinessLogic.Roles.Service;

public interface IRoleService
{
    public Guid Create(RoleArgs args);
    public Role? Get(Guid id);
    public List<Role> GetAll();
    public void Remove(Guid id);
    public void Update(RoleArgs args, Guid userId);
}
