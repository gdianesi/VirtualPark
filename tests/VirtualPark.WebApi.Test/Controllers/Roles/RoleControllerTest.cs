using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.WebApi.Controllers.Roles;
using VirtualPark.WebApi.Controllers.Roles.ModelsIn;
using VirtualPark.WebApi.Controllers.Roles.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Roles;

[TestClass]
public class RoleControllerTest
{
    private Mock<IRoleService> _roleServiceMock = null!;
    private RoleController _roleController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _roleServiceMock = new Mock<IRoleService>(MockBehavior.Strict);
        _roleController = new RoleController(_roleServiceMock.Object);
    }
    #region Create
    [TestMethod]
    public void CreateRole_ValidInput_ReturnsCreatedRoleResponse()
    {
        var p1 = Guid.NewGuid().ToString();
        var p2 = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateRoleRequest
        {
            Name = "Admin",
            Description = "Full access",
            PermissionsIds = [p1, p2]
        };

        var expectedArgs = request.ToArgs();

        _roleServiceMock
            .Setup(s => s.Create(It.Is<RoleArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Description == expectedArgs.Description &&
                a.PermissionIds.Count == expectedArgs.PermissionIds.Count &&
                a.PermissionIds.All(expectedArgs.PermissionIds.Contains))))
            .Returns(returnId);

        var response = _roleController.CreateRole(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateRoleResponse>();
        response.Id.Should().Be(returnId.ToString());

        _roleServiceMock.VerifyAll();
    }
    #endregion
    #region GetById
    [TestMethod]
    public void GetRoleById_ValidInput_ReturnsGetRoleResponse()
    {
        var perm1 = new Permission { Key = "users.read" };
        var perm2 = new Permission { Key = "users.write" };

        var u1 = new User { Name = "Pepe", LastName = "Perez", Email = "pepe@mail.com", Password = "Password123!" };
        var u2 = new User { Name = "Ana", LastName = "Gomez", Email = "ana@mail.com", Password = "Password123!" };

        var role = new Role
        {
            Name = "Manager",
            Description = "Manage users and content",
            Permissions = [perm1, perm2],
            Users = [u1, u2]
        };

        var id = role.Id;

        _roleServiceMock
            .Setup(s => s.Get(id))
            .Returns(role);

        var response = _roleController.GetRoleById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetRoleResponse>();
        response.Id.Should().Be(id.ToString());
        response.Name.Should().Be("Manager");
        response.Description.Should().Be("Manage users and content");

        response.PermissionIds.Should().HaveCount(2);
        response.PermissionIds.Should().Contain([perm1.Id.ToString(), perm2.Id.ToString()]);

        response.UsersIds.Should().HaveCount(2);
        response.UsersIds.Should().Contain([u1.Id.ToString(), u2.Id.ToString()]);

        _roleServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetRoleById_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";

        Action act = () => _roleController.GetRoleById(invalidId);

        act.Should().Throw<FormatException>();
        _roleServiceMock.VerifyNoOtherCalls();
    }
    #endregion

}
