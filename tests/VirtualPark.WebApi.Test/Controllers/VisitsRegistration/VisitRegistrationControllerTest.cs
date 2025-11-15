using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.BusinessLogic.VisitsScore.Models;
using VirtualPark.WebApi.Controllers.VisitsRegistration;
using VirtualPark.WebApi.Controllers.VisitsScore.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.VisitsRegistration;

[TestClass]
[TestCategory("Controller")]
[TestCategory("VisitScoresController")]
public class VisitRegistrationControllerTest
{
    private Mock<IVisitRegistrationService> _svc = null!;
    private VisitRegistrationController _controller = null!;

    [TestInitialize]
    public void Init()
    {
        _svc = new Mock<IVisitRegistrationService>(MockBehavior.Strict);
        _controller = new VisitRegistrationController(_svc.Object);
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
            a.Points == null), token));

        var result = _controller.RecordScoreEvent(body, token.ToString());

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
            a.Points == -50), token));

        var result = _controller.RecordScoreEvent(body, token.ToString());

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
            a.Points == null), token));

        var result = _controller.RecordScoreEvent(body, token.ToString());

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }
    #endregion

    #region UpToAttraction
    [TestMethod]
    [TestCategory("HappyPath")]
    public void UpToAttraction_ShouldReturnNoContent_WhenValid()
    {
        var visitId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        _svc.Setup(s => s.UpToAttraction(visitId, attractionId));

        var result = _controller.UpToAttraction(visitId.ToString(), attractionId.ToString());

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }

    [TestMethod]
    [TestCategory("HappyPath")]
    public void UpToAttraction_ShouldReturnNoContent_WithUppercaseAndLowercaseGuids()
    {
        var visitId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        _svc.Setup(s => s.UpToAttraction(visitId, attractionId));

        var result = _controller.UpToAttraction(visitId.ToString().ToUpperInvariant(),
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
        var visitId = Guid.NewGuid();

        _svc.Setup(s => s.DownToAttraction(visitId));

        var result = _controller.DownToAttraction(visitId.ToString());

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
}
