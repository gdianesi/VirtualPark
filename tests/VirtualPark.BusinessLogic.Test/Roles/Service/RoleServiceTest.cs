using System.Linq.Expressions;
using FluentAssertions;
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
    private RoleService _roleService = null!;
    private Mock<IReadOnlyRepository<Permission>> _mockPermissionReadOnlyRepository = null!;
    private RoleArgs _roleArgs = null!;
    private Mock<IRepository<Role>> _mockRoleRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockPermissionReadOnlyRepository = new Mock<IReadOnlyRepository<Permission>>(MockBehavior.Strict);
        _mockRoleRepository = new Mock<IRepository<Role>>(MockBehavior.Strict);
        _roleService = new RoleService(_mockRoleRepository.Object, _mockPermissionReadOnlyRepository.Object);

        var permissions = new List<string>() { Guid.NewGuid().ToString() };

        _roleArgs = new RoleArgs("Visitor", "Description", permissions);
    }

    #region GuidToPermission
    [TestMethod]
    public void GuidToPermission_WithValidIds_ReturnsPermissionsInOrder()
    {
        var p1 = new Permission { Key = "Read", Description = "Read permission" };
        var p2 = new Permission { Key = "Write", Description = "Write permission" };
        var data = new[] { p1, p2 }.AsQueryable();

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Expression<Func<Permission, bool>> pred) => data.FirstOrDefault(pred));

        var ids = new List<Guid> { p1.Id, p2.Id };

        var result = _roleService.GuidToPermission(ids);

        result.Should().HaveCount(2);
        result.Select(x => x.Id).Should().ContainInOrder(p1.Id, p2.Id);

        _mockPermissionReadOnlyRepository
            .Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()));
    }

    [TestMethod]
    public void GuidToPermission_WithNullList_ShouldThrowArgumentNullException()
    {
        Action act = () => _roleService.GuidToPermission(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void GuidToPermission_WhenAnyIdDoesNotExist_ShouldThrowKeyNotFoundException()
    {
        var existing = new Permission { Key = "OnlyOne", Description = "Only one exists" };
        var missingId = Guid.NewGuid();
        var data = new[] { existing }.AsQueryable();

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Expression<Func<Permission, bool>> pred) => data.FirstOrDefault(pred));

        var ids = new List<Guid> { existing.Id, missingId };

        Action act = () => _roleService.GuidToPermission(ids);

        act.Should().Throw<KeyNotFoundException>()
           .WithMessage($"Permission with id {missingId} does not exist");

        _mockPermissionReadOnlyRepository
            .Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()));
    }
    #endregion

    #region MapToEntity
    [TestMethod]
    public void MapToEntity_WhenArgsIsNull_ThrowsArgumentNullException()
    {
        Action act = () => _roleService.MapToEntity(null!);
        act.Should().Throw<ArgumentNullException>();
        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }

    [TestMethod]
    public void MapToEntity_WithValidArgs_MapsFields_AndResolvesPermissions()
    {
        var p1 = new Permission { Key = "Read", Description = "Read permission" };
        var p2 = new Permission { Key = "Write", Description = "Write permission" };
        var table = new[] { p1, p2 }.AsQueryable();

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Expression<Func<Permission, bool>> pred) => table.FirstOrDefault(pred));

        var args = new RoleArgs(
            name: "Visitor",
            description: "Description",
            permissions: [p1.Id.ToString(), p2.Id.ToString()]);

        Role result = _roleService.MapToEntity(args);

        result.Should().NotBeNull();
        result.Name.Should().Be("Visitor");
        result.Description.Should().Be("Description");
        result.Permissions.Should().HaveCount(2);
        result.Permissions.Select(x => x.Id).Should().ContainInOrder(p1.Id, p2.Id);

        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Exactly(2));
    }

    [TestMethod]
    public void MapToEntity_WithNoPermissions_ReturnsEmptyPermissions_AndNoRepoCalls()
    {
        var args = new RoleArgs(
            name: "Visitor",
            description: "Description",
            permissions: []);

        Role result = _roleService.MapToEntity(args);

        result.Should().NotBeNull();
        result.Name.Should().Be("Visitor");
        result.Description.Should().Be("Description");
        result.Permissions.Should().NotBeNull().And.BeEmpty();

        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }
    #endregion

    #region ValidateRoleName
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ValidateRoleName_WhenNullOrWhiteSpace_ThrowsArgumentException(string? name)
    {
        Action act = () => _roleService.ValidateRoleName(name!);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Role name cannot be empty*")
            .And.ParamName.Should().Be("name");

        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
    }

    [TestMethod]
    public void ValidateRoleName_WhenNameAlreadyExists_ThrowsException()
    {
        var existing = new Role { Name = "Admin" };
        var data = new[] { existing }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        Action act = () => _roleService.ValidateRoleName("admin");

        act.Should().Throw<Exception>()
            .WithMessage("Role name already exists.");

        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void ValidateRoleName_WithNewName_DoesNotThrow_AndQueriesRepository()
    {
        var data = Array.Empty<Role>().AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        Action act = () => _roleService.ValidateRoleName("Managers");

        act.Should().NotThrow();
        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }
    #endregion

    #region ApplyArgsToEntity
    [TestMethod]
    public void ApplyArgsToEntity_MapsFields_AndResolvesPermissions()
    {
        var p1 = new Permission { Key = "Read", Description = "R" };
        var p2 = new Permission { Key = "Write", Description = "W" };
        var table = new[] { p1, p2 }.AsQueryable();

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Expression<Func<Permission, bool>> pred) => table.FirstOrDefault(pred));

        var args = new RoleArgs("Manager", "Desc", [p1.Id.ToString(), p2.Id.ToString()]);
        var role = new Role();

        _roleService.ApplyArgsToEntity(role, args);

        role.Name.Should().Be("Manager");
        role.Description.Should().Be("Desc");
        role.Permissions.Select(x => x.Id).Should().ContainInOrder(p1.Id, p2.Id);
        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Exactly(2));
    }

    [TestMethod]
    public void ApplyArgsToEntity_WithNoPermissions_SetsEmptyList_AndNoRepoCalls()
    {
        var args = new RoleArgs("Visitor", "Description", []);
        var role = new Role { Permissions = [new Permission { Key = "X", Description = "X" }] };

        _roleService.ApplyArgsToEntity(role, args);

        role.Name.Should().Be("Visitor");
        role.Description.Should().Be("Description");
        role.Permissions.Should().BeEmpty();
        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }
    #endregion

    #region Create
    [TestMethod]
    public void Create_Valid_AddsAndReturnsId()
    {
        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(false);

        Role? agregado = null;
        _mockRoleRepository
            .Setup(r => r.Add(It.IsAny<Role>()))
            .Callback<Role>(r => agregado = r);

        var args = new RoleArgs("Manager", "Desc", []);

        var id = _roleService.Create(args);

        id.Should().NotBeEmpty();
        agregado.Should().NotBeNull();
        agregado!.Id.Should().Be(id);
        agregado.Name.Should().Be("Manager");
        agregado.Description.Should().Be("Desc");

        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
        _mockRoleRepository.Verify(r => r.Add(It.IsAny<Role>()), Times.Once);
        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }

    [TestMethod]
    public void Create_NameExists_ThrowsAndDoesNotAdd()
    {
        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(true);

        var args = new RoleArgs("Admin", "Desc", []);

        Action act = () => _roleService.Create(args);

        act.Should().Throw<Exception>().WithMessage("Role name already exists.");
        _mockRoleRepository.Verify(r => r.Add(It.IsAny<Role>()), Times.Never);
        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenPredicateIsNull_ReturnsAllFromRepository()
    {
        var data = new List<Role> { new Role { Name = "Admin" }, new Role { Name = "User" } };

        _mockRoleRepository
            .Setup(r => r.GetAll(It.Is<Expression<Func<Role, bool>>?>(p => p == null)))
            .Returns(data);

        var result = _roleService.GetAll();

        result.Should().BeEquivalentTo(data);
        _mockRoleRepository.Verify(r => r.GetAll(null), Times.Once);
    }

    [TestMethod]
    public void GetAll_WithPredicate_ReturnsFilteredFromRepository()
    {
        var data = new List<Role> { new Role { Name = "Admin" }, new Role { Name = "User" } };

        _mockRoleRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Role, bool>>?>()))
            .Returns((Expression<Func<Role, bool>>? pred) => data.AsQueryable().Where(pred!).ToList());

        var result = _roleService.GetAll(r => r.Name == "Admin");

        result.Should().HaveCount(1);
        result.Single().Name.Should().Be("Admin");
        _mockRoleRepository.Verify(r => r.GetAll(It.IsAny<Expression<Func<Role, bool>>?>()), Times.Once);
    }
    #endregion

    #region Get
    [TestMethod]
    public void Get_WithMatchingPredicate_ReturnsRole()
    {
        var admin = new Role { Name = "Admin" };
        var user = new Role { Name = "User" };
        var table = new[] { admin, user }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => table.FirstOrDefault(pred));

        var result = _roleService.Get(r => r.Name == "Admin");

        result.Should().NotBeNull();
        result!.Name.Should().Be("Admin");
        _mockRoleRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void Get_WithNoMatch_ReturnsNull()
    {
        var table = new[] { new Role { Name = "Admin" } }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => table.FirstOrDefault(pred));

        var result = _roleService.Get(r => r.Name == "Manager");

        result.Should().BeNull();
        _mockRoleRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }
    #endregion

    #region Exists
    [TestMethod]
    public void Exists_WithMatchingPredicate_ReturnsTrue()
    {
        var data = new[] { new Role { Name = "Admin" }, new Role { Name = "User" } }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        var result = _roleService.Exists(r => r.Name == "Admin");

        result.Should().BeTrue();
        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void Exists_WithNoMatch_ReturnsFalse()
    {
        var data = new[] { new Role { Name = "Admin" } }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        var result = _roleService.Exists(r => r.Name == "Manager");

        result.Should().BeFalse();
        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_Valid_MapsAndCallsUpdate()
    {
        var role = new Role { Name = "Old", Description = "Old desc" };
        var roleId = role.Id;

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(false);

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) =>
                new[] { role }.AsQueryable().FirstOrDefault(pred));

        _mockRoleRepository
            .Setup(r => r.Update(It.IsAny<Role>()));

        var args = new RoleArgs("Manager", "Desc", []);

        _roleService.Update(args, roleId);

        role.Name.Should().Be("Manager");
        role.Description.Should().Be("Desc");
        _mockRoleRepository.Verify(r => r.Update(It.IsAny<Role>()), Times.Once);
    }

    [TestMethod]
    public void Update_WhenRoleNotFound_ThrowsInvalidOperationException()
    {
        var roleId = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(false);

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Role?)null);

        var args = new RoleArgs("Manager", "Desc", []);

        Action act = () => _roleService.Update(args, roleId);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage($"Role with id {roleId} not found.");
        _mockRoleRepository.Verify(r => r.Update(It.IsAny<Role>()), Times.Never);
    }

    [TestMethod]
    public void Update_WhenNameAlreadyExists_ThrowsAndDoesNotUpdate()
    {
        var roleId = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns(true);

        var args = new RoleArgs("Admin", "Desc", []);

        Action act = () => _roleService.Update(args, roleId);

        act.Should().Throw<Exception>()
           .WithMessage("Role name already exists.");
        _mockRoleRepository.Verify(r => r.Update(It.IsAny<Role>()), Times.Never);
    }
    #endregion

    #region Remove
    [TestMethod]
    public void Remove_WhenRoleExists_CallsRepositoryRemove()
    {
        var role = new Role { Name = "Old", Description = "Old desc" };
        var id = role.Id;
        var table = new[] { role }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => table.FirstOrDefault(pred));

        _mockRoleRepository
            .Setup(r => r.Remove(It.IsAny<Role>()));

        _roleService.Remove(id);

        _mockRoleRepository.Verify(r => r.Remove(It.Is<Role>(x => x.Id == id)), Times.Once);
    }

    [TestMethod]
    public void Remove_WhenRoleNotFound_ThrowsInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _mockRoleRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Role?)null);

        Action act = () => _roleService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Role with id {id} not found.");
        _mockRoleRepository.Verify(r => r.Remove(It.IsAny<Role>()), Times.Never);
    }
    #endregion
}
