using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
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

    [TestInitialize]
    public void Initialize()
    {
        _mockPermissionReadOnlyRepository = new Mock<IReadOnlyRepository<Permission>>(MockBehavior.Strict);
        _roleService = new RoleService(_mockPermissionReadOnlyRepository.Object);

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
}
