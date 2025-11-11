using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("EditUserRequest")]
public class EditUserRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var req = new EditUserRequest { Name = "Pepe" };
        req.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var req = new EditUserRequest { LastName = "Perez" };
        req.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var req = new EditUserRequest { Email = "pepe@gmail.com" };
        req.Email.Should().Be("pepe@gmail.com");
    }
    #endregion

    #region RolesIds
    [TestMethod]
    [TestCategory("Validation")]
    public void Roles_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var req = new EditUserRequest { RolesIds = [guid] };
        req.RolesIds.Should().Contain([guid]);
    }
    #endregion

    #region VisitorProfile
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Getter_ReturnsAssignedValue()
    {
        var vp = new CreateVisitorProfileRequest
        {
            DateOfBirth = "2002-07-30",
            Membership = "Standard",
            Score = "95"
        };

        var req = new EditUserRequest { VisitorProfile = vp };

        req.VisitorProfile.Should().NotBeNull();
        req.VisitorProfile!.DateOfBirth.Should().Be("2002-07-30");
        req.VisitorProfile.Membership.Should().Be("Standard");
        req.VisitorProfile.Score.Should().Be("95");
    }
    #endregion

    #region ToArgs - Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapAllFields_AndVisitorProfile_AndRoles()
    {
        var guid1 = Guid.NewGuid().ToString();
        var guid2 = Guid.NewGuid().ToString();

        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [guid1, guid2],
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2000-01-01",
                Membership = "Standard",
                Score = "85"
            }
        };

        var args = req.ToArgs();

        args.Should().NotBeNull();
        args.Name.Should().Be("Pepe");
        args.LastName.Should().Be("Perez");
        args.Email.Should().Be("pepe@mail.com");
        args.RolesIds.Should().HaveCount(2);
        args.RolesIds.Should().Contain(Guid.Parse(guid1));
        args.RolesIds.Should().Contain(Guid.Parse(guid2));

        args.VisitorProfile.Should().NotBeNull();
        args.VisitorProfile!.DateOfBirth.Should().Be(new DateOnly(2000, 1, 1));
        args.VisitorProfile.Membership.ToString().Should().Be("Standard");
        args.VisitorProfile.Score.Should().Be(85);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldReturnUserArgs_WhenVisitorProfileIsNull()
    {
        var roleId = Guid.NewGuid().ToString();

        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [roleId],
            VisitorProfile = null
        };

        var args = req.ToArgs();

        args.Should().NotBeNull();
        args.Name.Should().Be("Pepe");
        args.LastName.Should().Be("Perez");
        args.Email.Should().Be("pepe@mail.com");
        args.VisitorProfile.Should().BeNull();
        args.RolesIds.Should().Contain(Guid.Parse(roleId));
    }
    #endregion

    #region ToArgs - Failure (Roles)
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenRolesIdsIsNull()
    {
        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = null,
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2002-07-30",
                Membership = "Standard",
                Score = "10"
            }
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenRolesIdsIsEmpty()
    {
        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [],
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = "2002-07-30",
                Membership = "Standard",
                Score = "10"
            }
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Role list can't be null");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenAnyRoleIdIsEmpty()
    {
        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [Guid.NewGuid().ToString(), string.Empty],
            VisitorProfile = null
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<ArgumentException>();
    }
    #endregion

    #region ToArgs - Failure (Fields)
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenNameIsEmpty()
    {
        var req = new EditUserRequest
        {
            Name = string.Empty,
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [Guid.NewGuid().ToString()]
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenLastNameIsEmpty()
    {
        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = string.Empty,
            Email = "pepe@mail.com",
            RolesIds = [Guid.NewGuid().ToString()]
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenEmailIsEmpty()
    {
        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = string.Empty,
            RolesIds = [Guid.NewGuid().ToString()]
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<ArgumentException>();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenVisitorProfileFieldsAreEmpty()
    {
        var role = Guid.NewGuid().ToString();

        var req = new EditUserRequest
        {
            Name = "Pepe",
            LastName = "Perez",
            Email = "pepe@mail.com",
            RolesIds = [role],
            VisitorProfile = new CreateVisitorProfileRequest
            {
                DateOfBirth = string.Empty,
                Membership = string.Empty,
                Score = string.Empty
            }
        };

        Action act = () => req.ToArgs();

        act.Should().Throw<ArgumentException>();
    }
    #endregion
}
