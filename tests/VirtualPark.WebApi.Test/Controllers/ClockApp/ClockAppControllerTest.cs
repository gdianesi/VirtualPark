using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.ClocksApp.Models;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.WebApi.Controllers.ClockApp;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsIn;
using VirtualPark.WebApi.Controllers.ClockApp.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.ClockApp;

[TestClass]
[TestCategory("ClockAppController")]
public sealed class ClockAppControllerTest
{
    private Mock<IClockAppService> _clockAppServiceMock = null!;
    private ClockAppController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _clockAppServiceMock = new(MockBehavior.Strict);
        _controller = new ClockAppController(_clockAppServiceMock.Object);
    }

    #region Get
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetClock_WhenClockExists_ShouldReturnGetClockResponse()
    {
        var clock = new BusinessLogic.ClocksApp.Entity.ClockApp
        {
            Id = Guid.NewGuid(),
            DateSystem = new DateTime(2025, 10, 6, 22, 00, 00)
        };

        _clockAppServiceMock
            .Setup(s => s.Get())
            .Returns(clock);

        var result = _controller.GetClock();

        result.Should().NotBeNull();
        result.Should().BeOfType<GetClockResponse>();
        result.DateSystem.Should().Be(clock.DateSystem.ToString("yyyy-MM-ddTHH:mm:ss"));

        _clockAppServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetClock_ShouldFormatDateWithoutMilliseconds()
    {
        var clock = new BusinessLogic.ClocksApp.Entity.ClockApp
        {
            Id = Guid.NewGuid(),
            DateSystem = new DateTime(2025, 10, 6, 22, 00, 00, 523) // con milisegundos
        };

        _clockAppServiceMock
            .Setup(s => s.Get())
            .Returns(clock);

        var result = _controller.GetClock();

        result.Should().NotBeNull();
        result.DateSystem.Should().Be("2025-10-06T22:00:00");

        _clockAppServiceMock.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    [TestCategory("Behaviour")]
    public void UpdateClock_WhenValidRequest_ShouldCallServiceUpdate()
    {
        var request = new UpdateClockRequest
        {
            DateSystem = "2025-10-06T22:00:00"
        };

        var expectedArgs = request.ToArgs();

        _clockAppServiceMock
            .Setup(s => s.Update(It.Is<ClockAppArgs>(a =>
                a.SystemDateTime == expectedArgs.SystemDateTime)))
            .Verifiable();

        _controller.UpdateClock(request);

        _clockAppServiceMock.VerifyAll();
    }
    #endregion
}
