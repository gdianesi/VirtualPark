using FluentAssertions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Users.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("VisitorInAttractionResponse")]
public class VisitorInAttractionResponseTest
{
    #region VisitorProfileId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorProfileId_Getter_ReturnsAssignedValue()
    {
        var visitorProfileId = Guid.NewGuid();
        var response = new VisitorInAttractionResponse
        {
            VisitorProfileId = visitorProfileId
        };

        response.VisitorProfileId.Should().Be(visitorProfileId);
    }
    #endregion

    #region UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void UserId_Getter_ReturnsAssignedValue()
    {
        var userId = Guid.NewGuid();
        var response = new VisitorInAttractionResponse
        {
            UserId = userId
        };

        response.UserId.Should().Be(userId);
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var response = new VisitorInAttractionResponse
        {
            Name = "Pepe"
        };

        response.Name.Should().Be("Pepe");
    }
    #endregion

    #region LastName
    [TestMethod]
    [TestCategory("Validation")]
    public void LastName_Getter_ReturnsAssignedValue()
    {
        var response = new VisitorInAttractionResponse
        {
            LastName = "Pérez"
        };

        response.LastName.Should().Be("Pérez");
    }
    #endregion

    #region Score
    [TestMethod]
    [TestCategory("Validation")]
    public void Score_Getter_ReturnsAssignedValue()
    {
        var response = new VisitorInAttractionResponse
        {
            Score = 123
        };

        response.Score.Should().Be(123);
    }
    #endregion

    #region Membership
    [TestMethod]
    [TestCategory("Validation")]
    public void Membership_Getter_ReturnsAssignedValue()
    {
        var membership = (Membership)5;

        var response = new VisitorInAttractionResponse
        {
            Membership = membership
        };

        response.Membership.Should().Be(membership);
    }
    #endregion

    #region NfcId
    [TestMethod]
    [TestCategory("Validation")]
    public void NfcId_Getter_ReturnsAssignedValue()
    {
        var nfcId = Guid.NewGuid();

        var response = new VisitorInAttractionResponse
        {
            NfcId = nfcId
        };

        response.NfcId.Should().Be(nfcId);
    }
    #endregion

    #region UserId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitRegistrationId_Getter_ReturnsAssignedValue()
    {
        var visitRegistrationId = Guid.NewGuid();
        var response = new VisitorInAttractionResponse
        {
            VisitRegistrationId = visitRegistrationId
        };

        response.VisitRegistrationId.Should().Be(visitRegistrationId);
    }
    #endregion

    #region TicketType
    [TestMethod]
    [TestCategory("Validation")]
    public void TicketType_Getter_ReturnsAssignedValue()
    {
        var ticketType = (EntranceType)2;

        var response = new VisitorInAttractionResponse
        {
            TicketType = ticketType
        };

        response.TicketType.Should().Be(ticketType);
    }
    #endregion
}
