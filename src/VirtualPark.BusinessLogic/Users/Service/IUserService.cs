using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;

namespace VirtualPark.BusinessLogic.Users.Service;

public interface IUserService
{
    public Guid Create(UserArgs args);
    public User? Get(Guid id);
    public List<User> GetAll();
    public void Remove(Guid id);
    public void Update(UserArgs args, Guid userId);
    bool HasPermission(Guid userLoggedId, string requiredPermission);
}
