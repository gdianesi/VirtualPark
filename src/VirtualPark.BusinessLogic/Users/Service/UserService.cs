using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Users.Service;

public class UserService(IRepository<User> userRepository)
{
    private readonly IRepository<User> _userRepository = userRepository;

    public Guid Create(UserArgs args)
    {
        ValidateEmail(args);

        var user = MapToEntity(args);

        _userRepository.Add(user);
        return user.Id;
    }

    private void ValidateEmail(UserArgs args)
    {
        var isEmailTaken = _userRepository.Exist(u => u.Email == args.Email);
        if(isEmailTaken)
        {
            throw new InvalidOperationException("Email already exists");
        }
    }

    private static User MapToEntity(UserArgs args) => new User
    {
        Name = args.Name,
        LastName = args.LastName,
        Email = args.Email,
        Password = args.Password,

        // crear visitor profile y role
    };
}
