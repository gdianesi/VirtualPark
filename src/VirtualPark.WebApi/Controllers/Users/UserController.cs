using Microsoft.AspNetCore.Mvc;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.Validations.Services;
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

    [HttpGet("{id}")]
    public GetUserResponse GetUserById(string id)
    {
        var userId = ValidationServices.ValidateAndParseGuid(id);
        var user = _userService.Get(userId)!;

        var roles = user.Roles.Select(r => r.Id.ToString()).ToList();
        var visitorProfileId = user.VisitorProfileId?.ToString() ?? null;

        return new GetUserResponse(
            id: user.Id.ToString(),
            name: user.Name,
            lastName: user.LastName,
            email: user.Email,
            roles: roles!,
            visitorProfileId: visitorProfileId
        );
    }
}
