using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using VirtualPark.BusinessLogic.Permissions.Entity;
using VirtualPark.BusinessLogic.Roles.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Models;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Users.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("UserService")]
public class UserServiceTest
{
    private Mock<IReadOnlyRepository<Role>> _rolesRepositoryMock = null!;
    private UserService _userService = null!;
    private Mock<IRepository<User>> _usersRepositoryMock = null!;
    private Mock<IReadOnlyRepository<VisitorProfile>> _visitorProfileRepositoryMock = null!;
    private Mock<IVisitorProfileService> _visitorProfileServiceMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _usersRepositoryMock = new Mock<IRepository<User>>(MockBehavior.Strict);
        _rolesRepositoryMock = new Mock<IReadOnlyRepository<Role>>(MockBehavior.Strict);
        _visitorProfileServiceMock = new Mock<IVisitorProfileService>(MockBehavior.Strict);
        _visitorProfileRepositoryMock = new Mock<IReadOnlyRepository<VisitorProfile>>(MockBehavior.Strict);
        _userService = new UserService(_usersRepositoryMock.Object, _rolesRepositoryMock.Object,
            _visitorProfileServiceMock.Object, _visitorProfileRepositoryMock.Object);
    }

    #region Create

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void CreateUser_ShouldCreateUser_WhenEmailDoesNotExist()
    {
        var roleId = Guid.NewGuid();
        var roles = new List<string> { roleId.ToString() };

        var args = new UserArgs("Pepe", "Perez", "pepe2@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == roleId))
            .Returns(new Role { Name = "Administrator" });

        _usersRepositoryMock
            .Setup(r => r.Add(It.Is<User>(u =>
                u.Name == args.Name &&
                u.LastName == args.LastName &&
                u.Email == args.Email &&
                u.Password == args.Password)));

        Guid result = _userService.Create(args);

        result.Should().NotBeEmpty();

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void CreateUser_ShouldCreateUser_WithVisitorProfile_FromArgs()
    {
        var roleId = Guid.NewGuid();
        var roles = new List<string> { roleId.ToString() };

        var vpArgs = new VisitorProfileArgs("2000-01-01", "Standard", "85");
        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!", roles) { VisitorProfile = vpArgs };

        var vpId = Guid.NewGuid();
        var vpEntity = new VisitorProfile
        {
            Id = vpId,
            DateOfBirth = vpArgs.DateOfBirth,
            Membership = vpArgs.Membership
        };

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == roleId))
            .Returns(new Role { Name = "Visitor" });

        _visitorProfileServiceMock
            .Setup(s => s.Create(vpArgs))
            .Returns(vpEntity);

        _usersRepositoryMock
            .Setup(r => r.Add(It.Is<User>(u =>
                u.Name == args.Name &&
                u.LastName == args.LastName &&
                u.Email == args.Email &&
                u.Password == args.Password &&
                u.VisitorProfile != null &&
                u.VisitorProfileId == vpId &&
                u.VisitorProfile!.Id == vpId &&
                u.VisitorProfile!.DateOfBirth == vpArgs.DateOfBirth &&
                u.VisitorProfile!.Membership == vpArgs.Membership)));

        Guid result = _userService.Create(args);

        result.Should().NotBeEmpty();

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
        _visitorProfileServiceMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenEmailAlreadyExists_ThrowsInvalidOperationException()
    {
        var guid = Guid.NewGuid();
        var roles = new List<string> { guid.ToString() };

        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(true);

        Func<Guid> act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Email already exists");

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenRoleDoesNotExist_ThrowsInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var roles = new List<string> { id.ToString() };
        var args = new UserArgs("Ana", "Gomez", "ana@mail.com", "Password123!", roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == id))
            .Returns((Role?)null);

        Action act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Role with id {id} does not exist.");

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenHasVisitorRole_WithoutVisitorProfile_ShouldThrow()
    {
        var visitorRoleId = Guid.NewGuid();
        var roles = new List<string> { visitorRoleId.ToString() };

        var args = new UserArgs(
            "Pepe",
            "Perez",
            "pepe@mail.com",
            "Password123!",
            roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        _rolesRepositoryMock
            .Setup(r => r.Get(role => role.Id == visitorRoleId))
            .Returns(new Role { Name = "Visitor" });

        Func<Guid> act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("You have a visitor role but you don't have a visitor profile.");

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
        _visitorProfileServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenPasswordIsNull_ShouldThrowInvalidOperationException()
    {
        var roleId = Guid.NewGuid();
        var roles = new List<string> { roleId.ToString() };
        var args = new UserArgs("Pepe", "Perez", "pepe@mail.com", null, roles);

        _usersRepositoryMock
            .Setup(r => r.Exist(u => u.Email == args.Email))
            .Returns(false);

        Action act = () => _userService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Password is required for creating a user.");

        _usersRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldReturnUserWithVisitorProfile_WhenUserExists()
    {
        var vpId = Guid.NewGuid();

        var dbUser = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = vpId
        };

        Guid id = dbUser.Id;

        var dbVisitorProfile = new VisitorProfile
        {
            Id = vpId,
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard
        };

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
            .Returns(dbUser);

        _visitorProfileRepositoryMock
            .Setup(r => r.Get(vp => vp.Id == dbUser.VisitorProfileId))
            .Returns(dbVisitorProfile);

        User? result = _userService.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.VisitorProfileId.Should().Be(vpId);
        result.VisitorProfile.Should().NotBeNull();
        result.VisitorProfile!.Id.Should().Be(vpId);
        result.VisitorProfile.DateOfBirth.Should().Be(dbVisitorProfile.DateOfBirth);
        result.VisitorProfile.Membership.Should().Be(dbVisitorProfile.Membership);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenUserDoesNotExist()
    {
        var anyId = Guid.NewGuid();

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns((User?)null);

        Action act = () => _userService.Get(anyId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User doesn't exist");

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyNoOtherCalls();
        _rolesRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region GetAll

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldReturnUsers_AndUploadVisitorProfiles_WhenTheyExist()
    {
        var vpId1 = Guid.NewGuid();
        var vp1 = new VisitorProfile
        {
            Id = vpId1,
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard
        };
        var user1 = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = vpId1,
            VisitorProfile = vp1
        };

        var usersFromRepo = new List<User> { user1 };

        _usersRepositoryMock
            .Setup(r => r.GetAll(
                (Expression<Func<User, bool>>?)null,
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(usersFromRepo);

        List<User> result = _userService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        User r1 = result[0];
        r1.Id.Should().Be(user1.Id);
        r1.VisitorProfileId.Should().Be(vpId1);
        r1.VisitorProfile.Should().NotBeNull();
        r1.VisitorProfile!.Id.Should().Be(vpId1);
        r1.VisitorProfile.DateOfBirth.Should().Be(vp1.DateOfBirth);
        r1.VisitorProfile.Membership.Should().Be(vp1.Membership);

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldReturnUsers_WhenTheyExist()
    {
        var user1 = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = null
        };

        var usersFromRepo = new List<User> { user1 };

        _usersRepositoryMock
            .Setup(r => r.GetAll(
                (Expression<Func<User, bool>>?)null,
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(usersFromRepo);

        List<User> result = _userService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(1);

        User r1 = result[0];
        r1.Id.Should().Be(user1.Id);

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _usersRepositoryMock
            .Setup(r => r.GetAll(
                (Expression<Func<User, bool>>?)null,
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns((List<User>)null!);

        Action act = () => _userService.GetAll();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Dont have any users");

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region remove

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldDeleteUser_WhenUserExists()
    {
        var dbUser = new User { Name = "Pepe", LastName = "Perez", Email = "pepe@mail.com", Password = "Password123!" };
        Guid id = dbUser.Id;

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
            .Returns(dbUser);

        _usersRepositoryMock
            .Setup(r => r.Remove(dbUser));

        _userService.Remove(id);

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldDeleteVisitorProfile_AndThenUser_WhenUserHasVisitorProfileId()
    {
        var visitorProfileId = Guid.NewGuid();
        var dbUser = new User
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            VisitorProfileId = visitorProfileId
        };
        Guid id = dbUser.Id;

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
            .Returns(dbUser);

        _visitorProfileServiceMock
            .Setup(s => s.Remove(visitorProfileId));

        _usersRepositoryMock
            .Setup(r => r.Remove(dbUser));

        _userService.Remove(id);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileServiceMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldThrow_WhenUserDoesNotExist()
    {
        var id = Guid.NewGuid();

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns((User?)null);

        Action act = () => _userService.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User doesn't exist");
        _usersRepositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Update
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Update_ShouldUpdateBasicFields_WhenArgsWithoutVisitorProfile()
    {
        var existingVp = new VisitorProfile
        {
            DateOfBirth = new DateOnly(1990, 1, 1),
            Membership = Membership.Standard
        };
        Guid vpId = existingVp.Id;
        var existingUser = new User
        {
            Name = "OldName",
            LastName = "OldLast",
            Email = "user@mail.com",
            Password = "Password1!",
            VisitorProfileId = vpId
        };
        Guid userId = existingUser.Id;
        var args = new UserArgs(
            "NewName",
            "NewLast",
            "user@mail.com",
            "Password1!",
            [])
        { VisitorProfile = null };
        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(existingUser);
        _visitorProfileRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(existingVp);

        _usersRepositoryMock
            .Setup(r => r.Update(It.Is<User>(u =>
                u.Id == userId &&
                u.Name == args.Name &&
                u.LastName == args.LastName &&
                u.Password == args.Password &&
                u.Email == "user@mail.com" &&
                u.VisitorProfileId == vpId &&
                u.VisitorProfile != null &&
                u.VisitorProfile.Id == vpId &&
                u.VisitorProfile.DateOfBirth == existingVp.DateOfBirth &&
                u.VisitorProfile.Membership == existingVp.Membership)))
            .Verifiable();

        _userService.Update(args, userId);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Update_ShouldUpdateVisitorProfile_WhenArgsHasVisitorProfile()
    {
        var existingVp = new VisitorProfile
        {
            DateOfBirth = new DateOnly(1990, 1, 1),
            Membership = Membership.Standard,
            Score = 10
        };
        Guid vpId = existingVp.Id;

        var existingUser = new User
        {
            Name = "OldName",
            LastName = "OldLast",
            Email = "user@mail.com",
            Password = "OldPass1!",
            VisitorProfileId = vpId
        };
        Guid userId = existingUser.Id;

        var newVpArgs = new VisitorProfileArgs("2002-07-30", "Premium", "85");

        var args = new UserArgs(
            "NewName",
            "NewLast",
            "user@mail.com",
            "NewPass1!",
            [])
        { VisitorProfile = newVpArgs };

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(existingUser);

        _visitorProfileRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(existingVp);

        _visitorProfileServiceMock
            .Setup(s => s.Update(
                It.Is<VisitorProfileArgs>(a =>
                    a.DateOfBirth == newVpArgs.DateOfBirth &&
                    a.Membership == newVpArgs.Membership &&
                    a.Score == newVpArgs.Score),
                vpId))
            .Verifiable();

        _usersRepositoryMock
            .Setup(r => r.Update(It.IsAny<User>()))
            .Verifiable();

        _userService.Update(args, userId);

        _usersRepositoryMock.VerifyAll();
        _visitorProfileRepositoryMock.VerifyAll();
        _visitorProfileServiceMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void Update_ShouldThrow_WhenUserDoesNotExist()
    {
        var args = new UserArgs(
            "Any",
            "Body",
            "user@mail.com",
            "Password123!",
            []);

        var userId = Guid.NewGuid();

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns((User?)null);

        Action act = () => _userService.Update(args, userId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User doesn't exist");

        _usersRepositoryMock.VerifyAll();
        _usersRepositoryMock.Verify(r => r.Update(It.IsAny<User>()), Times.Never);
        _visitorProfileRepositoryMock.VerifyNoOtherCalls();
        _rolesRepositoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region HasPermission

    #region Success

    [TestMethod]
    [TestCategory("Validation")]
    public void HasPermission_ShouldReturnTrue_WhenFirstRoleHasThePermission()
    {
        var userId = Guid.NewGuid();

        var role1 = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Permissions = [new Permission { Key = "USERS_MANAGE", Description = "Manage" }]
        };

        var user = new User
        {
            Id = userId,
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            Password = "Password123!",
            Roles = [role1]
        };

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(user);

        var result = _userService.HasPermission(userId, "USERS_MANAGE");

        result.Should().BeTrue();

        _usersRepositoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void HasPermission_ShouldReturnTrue_WhenAnyRoleHasThePermission()
    {
        var userId = Guid.NewGuid();

        var role1 = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Visitor",
            Permissions = [new Permission { Key = "OTHER_PERMISSION" }]
        };

        var role2 = new Role
        {
            Id = Guid.NewGuid(),
            Name = "Admin",
            Permissions = [new Permission { Key = "USERS_MANAGE" }]
        };

        var user = new User
        {
            Id = userId,
            Name = "Ana",
            LastName = "Gomez",
            Email = "ana@mail.com",
            Password = "Password123!",
            Roles = [role1, role2]
        };

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns(user);

        var result = _userService.HasPermission(userId, "USERS_MANAGE");

        result.Should().BeTrue();

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void HasPermission_ShouldReturnFalse_WhenUserHasNoRoles()
    {
        var userId = Guid.NewGuid();

        var user = new User
        {
            Name = "Sin",
            LastName = "Roles",
            Email = "noroles@mail.com",
            Password = "Password123!",
            Roles = []
        };

        _usersRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>?>()))
            .Returns(user);

        var result = _userService.HasPermission(userId, "USERS_MANAGE");

        result.Should().BeFalse();

        _usersRepositoryMock.VerifyAll();
        _rolesRepositoryMock.VerifyAll();
    }

    #endregion

    #endregion

    #region GetByVisitorProfileId
    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitorProfileId_WhenUserExists_ShouldReturnUser()
    {
        var visitorProfileId = Guid.NewGuid();

        var user = new User
        {
            VisitorProfileId = visitorProfileId,
            Name = "Ana",
            LastName = "Pérez",
            Email = "ana@example.com"
        };

        _usersRepositoryMock
            .Setup(r => r.Get(u => u.VisitorProfileId == visitorProfileId))
            .Returns(user);

        var result = _userService.GetByVisitorProfileId(visitorProfileId);

        result.Should().NotBeNull();
        result.Should().BeSameAs(user);
        result.VisitorProfileId.Should().Be(visitorProfileId);
        result.Name.Should().Be("Ana");
        result.LastName.Should().Be("Pérez");

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitorProfileId_WhenUserDoesNotExist_ShouldThrow()
    {
        var visitorProfileId = Guid.NewGuid();

        _usersRepositoryMock
            .Setup(r => r.Get(u => u.VisitorProfileId == visitorProfileId))
            .Returns((User?)null);

        Action act = () => _userService.GetByVisitorProfileId(visitorProfileId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("User for VisitorProfile not found");

        _usersRepositoryMock.VerifyAll();
    }

    #endregion

    #region GetByVisitorProfileIds

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitorProfileIds_WhenListIsEmpty_ShouldReturnEmptyList()
    {
        var ids = new List<Guid>();

        var result = _userService.GetByVisitorProfileIds(ids);

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitorProfileIds_WhenUsersExist_ShouldReturnMatchingUsers()
    {
        var vpId1 = Guid.NewGuid();
        var vpId2 = Guid.NewGuid();

        var ids = new List<Guid> { vpId1, vpId2 };

        var user1 = new User
        {
            VisitorProfileId = vpId1,
            Name = "Ana",
            LastName = "Pérez",
            Email = "ana@example.com"
        };

        var user2 = new User
        {
            VisitorProfileId = vpId2,
            Name = "Luis",
            LastName = "Gómez",
            Email = "luis@example.com"
        };

        _usersRepositoryMock
            .Setup(r => r.GetAll(u => u.VisitorProfileId.HasValue &&
                                      ids.Contains(u.VisitorProfileId.Value)))
            .Returns([user1, user2]);

        var result = _userService.GetByVisitorProfileIds(ids);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(user1);
        result.Should().Contain(user2);

        result.Should().Contain(u => u.VisitorProfileId == vpId1 && u.Name == "Ana");
        result.Should().Contain(u => u.VisitorProfileId == vpId2 && u.Name == "Luis");

        _usersRepositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetByVisitorProfileIds_WhenNoUsersFound_ShouldReturnEmptyList()
    {
        var vpId1 = Guid.NewGuid();
        var vpId2 = Guid.NewGuid();

        var ids = new List<Guid> { vpId1, vpId2 };

        _usersRepositoryMock
            .Setup(r => r.GetAll(u => u.VisitorProfileId.HasValue &&
                                      ids.Contains(u.VisitorProfileId.Value)))
            .Returns([]);

        var result = _userService.GetByVisitorProfileIds(ids);

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _usersRepositoryMock.VerifyAll();
    }

    #endregion
}
