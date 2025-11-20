using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.Users.Service;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.BusinessLogic.VisitsScore.Models;
using VirtualPark.WebApi.Controllers.Users.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitsRegistration;
using VirtualPark.WebApi.Controllers.VisitsRegistration.ModelsOut;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.VisitsRegistration;

[TestClass]
[TestCategory("Controller")]
[TestCategory("VisitScoresController")]
public class VisitRegistrationControllerTest
{
    private Mock<IVisitRegistrationService> _svc = null!;
    private VisitRegistrationController _controller = null!;
    private Mock<IUserService> _userSvc = null!;

    [TestInitialize]
    public void Init()
    {
        _svc = new Mock<IVisitRegistrationService>(MockBehavior.Strict);
        _userSvc = new Mock<IUserService>(MockBehavior.Strict);
        _controller = new VisitRegistrationController(_svc.Object, _userSvc.Object);
    }

    #region RecosrdScoreEvent
    [TestMethod]
    [TestCategory("HappyPath")]
    public void RecordScoreEvent_ShouldReturnNoContent_WhenValid_NoPoints()
    {
        var visitId = Guid.NewGuid();
        var body = new VisitScoreRequest { VisitRegistrationId = visitId.ToString(), Origin = "Atracción", Points = null };
        var token = Guid.NewGuid();

        _svc.Setup(s => s.RecordVisitScore(It.Is<RecordVisitScoreArgs>(a =>
            a.VisitRegistrationId == visitId &&
            a.Origin == "Atracción" &&
            a.Points == null)));

        var result = _controller.RecordScoreEvent(body);

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }

    [TestMethod]
    [TestCategory("HappyPath")]
    public void RecordScoreEvent_ShouldReturnNoContent_WhenValid_WithPoints()
    {
        var visitId = Guid.NewGuid();
        var body = new VisitScoreRequest { VisitRegistrationId = visitId.ToString(), Origin = "Canje", Points = "-50" };
        var token = Guid.NewGuid();

        _svc.Setup(s => s.RecordVisitScore(It.Is<RecordVisitScoreArgs>(a =>
            a.VisitRegistrationId == visitId &&
            a.Origin == "Canje" &&
            a.Points == -50)));

        var result = _controller.RecordScoreEvent(body);

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }

    [TestMethod]
    [TestCategory("HappyPath")]
    public void RecordScoreEvent_ShouldReturnNoContent_WhenValid_OriginIsTrimmed()
    {
        var visitId = Guid.NewGuid();
        var body = new VisitScoreRequest { VisitRegistrationId = visitId.ToString(), Origin = "  Atracción  ", Points = null };
        var token = Guid.NewGuid();

        _svc.Setup(s => s.RecordVisitScore(It.Is<RecordVisitScoreArgs>(a =>
            a.VisitRegistrationId == visitId &&
            a.Origin == "Atracción" &&
            a.Points == null)));

        var result = _controller.RecordScoreEvent(body);

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }
    #endregion

    #region UpToAttraction
    [TestMethod]
    [TestCategory("HappyPath")]
    public void UpToAttraction_ShouldReturnNoContent_WhenValid()
    {
        var visitorId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        _svc.Setup(s => s.UpToAttraction(visitorId, attractionId));

        var result = _controller.UpToAttraction(visitorId.ToString(), attractionId.ToString());

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }

    [TestMethod]
    [TestCategory("HappyPath")]
    public void UpToAttraction_ShouldReturnNoContent_WithUppercaseAndLowercaseGuids()
    {
        var visitorId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        _svc.Setup(s => s.UpToAttraction(visitorId, attractionId));

        var result = _controller.UpToAttraction(
            visitorId.ToString().ToUpperInvariant(),
            attractionId.ToString().ToLowerInvariant());

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }
    #endregion

    #region DownToAttraction
    [TestMethod]
    [TestCategory("HappyPath")]
    public void DownToAttraction_ShouldReturnNoContent_WhenValid()
    {
        var visitorId = Guid.NewGuid();

        _svc.Setup(s => s.DownToAttraction(visitorId));

        var result = _controller.DownToAttraction(visitorId.ToString());

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }
    #endregion

    #region GetAttractionsForTicket
    [TestMethod]
    [TestCategory("HappyPath")]
    public void GetAttractionsForTicket_ShouldReturnOk_WithAttractions()
    {
        var visitorId = Guid.NewGuid();

        var a1 = new Attraction { Name = "Roller" };
        var a2 = new Attraction { Name = "Wheel" };
        var attractions = new List<Attraction> { a1, a2 };

        _svc
            .Setup(s => s.GetAttractionsForTicket(visitorId))
            .Returns(attractions);

        var result = _controller.GetAttractionsForTicket(visitorId.ToString());

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        ok.Value.Should().BeSameAs(attractions);

        _svc.VerifyAll();
    }
    #endregion

    #region GetVisitorsInAttraction
    [TestMethod]
    [TestCategory("HappyPath")]
    public void GetVisitorsInAttraction_ShouldReturnOk_WithMappedUsers()
    {
        var attractionId = Guid.NewGuid();

        var vp1 = new VisitorProfile { Score = 10, Membership = Membership.Standard };
        var vp2 = new VisitorProfile { Score = 20, Membership = Membership.Premium };

        var via1 = new VisitorInAttraction
        {
            VisitRegistrationId = Guid.NewGuid(),
            Visitor = vp1
        };

        var via2 = new VisitorInAttraction
        {
            VisitRegistrationId = Guid.NewGuid(),
            Visitor = vp2
        };

        var user1 = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = vp1.Id,
            Name = "Ana",
            LastName = "Pérez"
        };

        var user2 = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfileId = vp2.Id,
            Name = "Luis",
            LastName = "Gómez"
        };

        var visitSvcMock = new Mock<IVisitRegistrationService>(MockBehavior.Strict);
        var userSvcMock = new Mock<IUserService>(MockBehavior.Strict);

        visitSvcMock
            .Setup(s => s.GetVisitorsInAttraction(attractionId))
            .Returns([via1, via2]);

        userSvcMock
            .Setup(s => s.GetByVisitorProfileIds(It.Is<List<Guid>>(ids =>
                ids.Count == 2 && ids.Contains(vp1.Id) && ids.Contains(vp2.Id))))
            .Returns([user1, user2]);

        var controller = new VisitRegistrationController(visitSvcMock.Object, userSvcMock.Object);

        var result = controller.GetVisitorsInAttraction(attractionId.ToString());

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var list = ok.Value.Should().BeAssignableTo<List<VisitorInAttractionResponse>>().Subject;

        list.Should().HaveCount(2);

        list.Should().ContainSingle(v =>
            v.VisitorProfileId == vp1.Id &&
            v.UserId == user1.Id &&
            v.Name == "Ana" &&
            v.LastName == "Pérez" &&
            v.Score == vp1.Score &&
            v.Membership == vp1.Membership &&
            v.NfcId == vp1.NfcId);

        list.Should().ContainSingle(v =>
            v.VisitorProfileId == vp2.Id &&
            v.UserId == user2.Id &&
            v.Name == "Luis" &&
            v.LastName == "Gómez" &&
            v.Score == vp2.Score &&
            v.Membership == vp2.Membership &&
            v.NfcId == vp2.NfcId);

        visitSvcMock.VerifyAll();
        userSvcMock.VerifyAll();
    }
    #endregion

    #region GetVisitForToday
    [TestMethod]
    [TestCategory("HappyPath")]
    public void GetVisitForToday_ShouldReturnOk_WithVisitId()
    {
        var visitorId = Guid.NewGuid();
        var visit = new VisitRegistration();

        _svc.Setup(s => s.GetTodayVisit(visitorId))
            .Returns(visit);

        var result = _controller.GetVisitForToday(visitorId.ToString());

        var ok = result.Should().BeOfType<OkObjectResult>().Subject;
        var payload = ok.Value.Should().BeOfType<VisitRegistrationTodayResponse>().Subject;
        payload.VisitRegistrationId.Should().Be(visit.Id);

        _svc.VerifyAll();
    }
    #endregion

}
