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

    [HttpGet("users/{id}")]
    public GetUserResponse GetUserById(string id)
    {
        var userId = ValidationServices.ValidateAndParseGuid(id);

        var user = _userService.Get(userId)!;

        return new GetUserResponse(
            id: user.Id.ToString(),
            name: user.Name,
            lastName: user.LastName,
            email: user.Email,
            roles: user.Roles.Select(r => r.Id.ToString()).ToList(),
            visitorProfileId: user.VisitorProfileId?.ToString() ?? null);
    }

    [HttpGet]
    public List<GetUserResponse> GetAllUsers()
    {
        return _userService.GetAll()
            .Select(u => new GetUserResponse(
                id: u.Id.ToString(),
                name: u.Name,
                lastName: u.LastName,
                email: u.Email,
                roles: u.Roles.Select(r => r.Id.ToString()).ToList(),
                visitorProfileId: u.VisitorProfileId?.ToString() ?? null))
            .ToList();
    }

    [HttpDelete("users/{id}")]
    public void DeleteUser(string id)
    {
        var userId = ValidationServices.ValidateAndParseGuid(id);
        _userService.Remove(userId);
    }

    [HttpPatch("users/{id}")]
    public void UpdateUser(CreateUserRequest request, string id)
    {
        var userId = ValidationServices.ValidateAndParseGuid(id);

        UserArgs args = request.ToArgs();

        _userService.Update(args, userId);
    }
}
