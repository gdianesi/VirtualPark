using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Roles.Models;
using VirtualPark.BusinessLogic.Roles.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Roles.Service;

[TestClass]
[TestCategory("RoleService")]
public sealed class RoleServiceTest
{
    private Mock<IReadOnlyRepository<Permission>> _mockPermissionReadOnlyRepository = null!;
    private Mock<IRepository<Role>> _mockRoleRepository = null!;
    private RoleService _roleService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockPermissionReadOnlyRepository = new Mock<IReadOnlyRepository<Permission>>(MockBehavior.Strict);
        _mockRoleRepository = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _roleService = new RoleService(_mockRoleRepository.Object, _mockPermissionReadOnlyRepository.Object);
        _ = new List<string> { Guid.NewGuid().ToString() };
    }

    #region GetAll

    [TestMethod]
    public void GetAll_WhenRepositoryReturnsList_ShouldReturnSameList()
    {
        var data = new List<Role> { new() { Name = "Admin" }, new() { Name = "User" } };

        _mockRoleRepository
            .Setup(r => r.GetAll(null))
            .Returns(data);

        List<Role> result = _roleService.GetAll();

        result.Should().BeEquivalentTo(data);
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    #endregion

    #region Create

    [TestMethod]
    public void Create_ShouldAddRole_WhenNameIsNew_AndPermissionsExist()
    {
        var p1 = new Permission { Key = "READ", Description = "R" };
        var p2 = new Permission { Key = "WRITE", Description = "W" };
        Guid p1Id = p1.Id;
        Guid p2Id = p2.Id;

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(false);

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(p => p.Id == p1Id))
            .Returns(p1);

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(p => p.Id == p2Id))
            .Returns(p2);

        _mockRoleRepository
            .Setup(r => r.Add(It.Is<Role>(x =>
                x.Name == "Manager" &&
                x.Description == "Desc" &&
                x.Permissions.Count == 2 &&
                x.Permissions[0].Id == p1Id &&
                x.Permissions[1].Id == p2Id)));

        var args = new RoleArgs(
            "Manager",
            "Desc",
            [p1Id.ToString(), p2Id.ToString()]);

        Guid id = _roleService.Create(args);

        id.Should().NotBeEmpty();
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Create_ShouldThrow_WhenNameAlreadyExists()
    {
        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(true);

        var args = new RoleArgs("Admin", "Desc", []);

        Func<Guid> act = () => _roleService.Create(args);

        act.Should().Throw<Exception>().WithMessage("Role name already exists.");
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    #endregion

    #region Get

    [TestMethod]
    public void Get_ShouldReturnRole_WhenExists()
    {
        var role = new Role { Name = "Admin" };
        Guid id = role.Id;

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns(role);

        Role result = _roleService.Get(id);

        result.Should().BeSameAs(role);
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_ShouldThrow_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns((Role?)null);

        Func<Role> act = () => _roleService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Role don't exist");
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    #endregion

    #region Update

    [TestMethod]
    public void Update_ShouldApplyChanges_AndPersist_WhenRoleExists_AndNameNew_AndPermissionsResolved()
    {
        var existing = new Role { Name = "Old", Description = "Old desc", Permissions = [] };
        Guid roleId = existing.Id;

        var p1 = new Permission { Key = "READ", Description = "R" };
        var p2 = new Permission { Key = "WRITE", Description = "W" };
        Guid p1Id = p1.Id;
        Guid p2Id = p2.Id;

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns(existing);

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(false);

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(p => p.Id == p1Id))
            .Returns(p1);
        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(p => p.Id == p2Id))
            .Returns(p2);

        _mockRoleRepository
            .Setup(r => r.Update(It.Is<Role>(x =>
                x.Id == roleId &&
                x.Name == "Manager" &&
                x.Description == "Desc" &&
                x.Permissions.Count == 2 &&
                x.Permissions[0].Id == p1Id &&
                x.Permissions[1].Id == p2Id)));

        var args = new RoleArgs(
            "Manager",
            "Desc",
            [p1Id.ToString(), p2Id.ToString()]);

        _roleService.Update(args, roleId);

        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_ShouldThrow_WhenRoleNotFound()
    {
        var id = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns((Role?)null);

        var args = new RoleArgs("Manager", "Desc", []);

        Action act = () => _roleService.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Role don't exist");
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_ShouldThrow_WhenNewNameAlreadyExists()
    {
        var existing = new Role { Name = "Old" };
        Guid id = existing.Id;

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns(existing);

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(true);

        var args = new RoleArgs("Admin", "Desc", []);

        Action act = () => _roleService.Update(args, id);

        act.Should().Throw<Exception>()
            .WithMessage("Role name already exists.");
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_ShouldThrow_WhenNameEmpty()
    {
        var roleId = Guid.NewGuid();

        Action act = () => _roleService.Update(new RoleArgs("   ", "Desc", []), roleId);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");

        _mockRoleRepository.VerifyNoOtherCalls();
        _mockPermissionReadOnlyRepository.VerifyNoOtherCalls();
    }

    #endregion

    #region Remove

    [TestMethod]
    public void Remove_ShouldDelete_WhenExists()
    {
        var role = new Role { Name = "X" };
        Guid id = role.Id;

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns(role);

        _mockRoleRepository
            .Setup(r => r.Remove(role));

        _roleService.Remove(id);

        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Remove_ShouldThrow_WhenNotFound()
    {
        var id = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>, IIncludableQueryable<Role, object>>>()))
            .Returns((Role?)null);

        Action act = () => _roleService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Role don't exist");
        _mockRoleRepository.VerifyAll();
        _mockPermissionReadOnlyRepository.VerifyAll();
    }

    #endregion
}
