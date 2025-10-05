using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Controllers.Users;

[ApiController]
public sealed class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("users")]
    public CreateUserResponse CreateUser(CreateUserRequest newUser)
    {
        UserArgs userArgs = newUser.ToArgs();

        Guid responseId = _userService.Create(userArgs);

        return new CreateUserResponse(responseId.ToString());
    }
}
