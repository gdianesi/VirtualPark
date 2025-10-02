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

        var permissions = new[] { Guid.NewGuid().ToString() };
        _roleArgs = new RoleArgs("Visitor", "Description", permissions);
    }

    #region GuidToPermission
    [TestMethod]
    public void GuidToPermission_WithValidIds_ReturnsPermissionsInOrder()
    {
        var p1 = new Permission { Key = "Read",  Description = "Read permission"  };
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
        var existing  = new Permission { Key = "OnlyOne", Description = "Only one exists" };
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
        var p1 = new Permission { Key = "Read",  Description = "Read permission"  };
        var p2 = new Permission { Key = "Write", Description = "Write permission" };
        var table = new[] { p1, p2 }.AsQueryable();

        _mockPermissionReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()))
            .Returns((Expression<Func<Permission, bool>> pred) => table.FirstOrDefault(pred));

        var args = new RoleArgs(
            name: "Visitor",
            description: "Description",
            permissions: new[] { p1.Id.ToString(), p2.Id.ToString() }
        );

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
            permissions: Array.Empty<string>()
        );

        Role result = _roleService.MapToEntity(args);

        result.Should().NotBeNull();
        result.Name.Should().Be("Visitor");
        result.Description.Should().Be("Description");
        result.Permissions.Should().NotBeNull().And.BeEmpty();

        _mockPermissionReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Permission, bool>>>()), Times.Never);
    }
    #endregion
    #region ValidateAttractionName
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void ValidateAttractionName_WhenNullOrWhiteSpace_ThrowsArgumentException(string? name)
    {
        // Act
        Action act = () => _roleService.ValidateAttractionName(name!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("Role name cannot be empty*")
            .And.ParamName.Should().Be("name");

        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Never);
    }

    [TestMethod]
    public void ValidateAttractionName_WhenNameAlreadyExists_ThrowsException()
    {
        var existing = new Role { Name = "Admin" };
        var data = new[] { existing }.AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        Action act = () => _roleService.ValidateAttractionName("admin");

        act.Should().Throw<Exception>()
            .WithMessage("Role name already exists.");

        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void ValidateAttractionName_WithNewName_DoesNotThrow_AndQueriesRepository()
    {
        var data = Array.Empty<Role>().AsQueryable();

        _mockRoleRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()))
            .Returns((Expression<Func<Role, bool>> pred) => data.Any(pred));

        Action act = () => _roleService.ValidateAttractionName("Managers");

        act.Should().NotThrow();
        _mockRoleRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Role, bool>>>()), Times.Once);
    }
    #endregion

}
