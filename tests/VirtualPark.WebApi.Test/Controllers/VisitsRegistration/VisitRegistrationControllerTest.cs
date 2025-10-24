using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
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

    [TestMethod]
    [TestCategory("HappyPath")]
    public void RecordScoreEvent_ShouldReturnNoContent_WhenValid_NoPoints()
    {
        var visitId = Guid.NewGuid();
        var body = new VisitScoreRequest { VisitRegistrationId = visitId.ToString(), Origin = "Atracción", Points = null };

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

        _svc.Setup(s => s.RecordVisitScore(It.Is<RecordVisitScoreArgs>(a =>
            a.VisitRegistrationId == visitId &&
            a.Origin == "Atracción" &&
            a.Points == null)));

        var result = _controller.RecordScoreEvent(body);

        result.Should().BeOfType<NoContentResult>();
        _svc.VerifyAll();
    }
}
