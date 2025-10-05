using FluentAssertions;
using VirtualPark.WebApi.Controllers.Users.ModelsIn;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateUserRequest")]
public class CreateUserRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var createUserRequest = new CreateUserRequest { Name = "Pepe" };
        createUserRequest.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var createUserRequest = new CreateUserRequest { LastName = "Perez" };
        createUserRequest.LastName.Should().Be("Perez");
    }
    #endregion

    #region Email
    [TestMethod]
    [TestCategory("Validation")]
    public void Email_Getter_ReturnsAssignedValue()
    {
        var createUserRequest = new CreateUserRequest { Email = "pepe@gmail.com" };
        createUserRequest.Email.Should().Be("pepe@gmail.com");
    }
    #endregion

    #region Password
    [TestMethod]
    [TestCategory("Validation")]
    public void Password_Getter_ReturnsAssignedValue()
    {
        var createUserRequest = new CreateUserRequest { Password = "Pepit@01" };
        createUserRequest.Password.Should().Be("Pepit@01");
    }
    #endregion

    #region RolesIds
    [TestMethod]
    [TestCategory("Validation")]
    public void Roles_Getter_ReturnsAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var createUserRequest = new CreateUserRequest { RolesIds = new List<string> { guid } };
        createUserRequest.RolesIds.Should().Contain([guid]);
    }
    #endregion

    #region VisitorProfile
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfile_Getter_ReturnsAssignedValue()
    {
        var visitorProfileRequest = new CreateVisitorProfileRequest
        {
            DateOfBirth = "2002-07-30",
            Membership = "Standard",
            Score = "95"
        };

        var createUserRequest = new CreateUserRequest
        {
            VisitorProfile = visitorProfileRequest
        };

        var result = createUserRequest.VisitorProfile;

        result.Should().NotBeNull();
        result!.DateOfBirth.Should().Be("2002-07-30");
        result.Membership.Should().Be("Standard");
        result.Score.Should().Be("95");
    }
    #endregion
}
