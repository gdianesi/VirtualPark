using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.ClocksApp.Service;

[TestClass]
[TestCategory("ClockAppService")]
public class ClockAppServiceTest
{
    private Mock<IRepository<ClockApp>> _clockAppRepository = null!;
    private ClockAppService _clockAppService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _clockAppRepository = new Mock<IRepository<ClockApp>>(MockBehavior.Strict);
        _clockAppService = new ClockAppService(_clockAppRepository.Object);
    }

    #region CalculateDifferenceInMinutes

    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenSameDateTime_ShouldReturnZero()
    {
        _clockAppRepository.Setup(r => r.GetAll(null)).Returns(new List<ClockApp>());
        _clockAppRepository.Setup(r => r.Add(It.IsAny<ClockApp>()));

        var service = new ClockAppService(_clockAppRepository.Object);

        var now = DateTime.Now;
        var result = service.CalculateDifferenceInMinutes(now);

        result.Should().Be(0);

        _clockAppRepository.Verify(r => r.GetAll(null), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenFutureDateTime_ShouldReturnPositive()
    {
        _clockAppRepository.Setup(r => r.GetAll(null)).Returns(new List<ClockApp>());
        _clockAppRepository.Setup(r => r.Add(It.IsAny<ClockApp>()));

        var service = new ClockAppService(_clockAppRepository.Object);

        var future = DateTime.Now.AddMinutes(30);
        var result = service.CalculateDifferenceInMinutes(future);

        result.Should().Be(30);

        _clockAppRepository.Verify(r => r.GetAll(null), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenPastDateTime_ShouldReturnNegative()
    {
        _clockAppRepository.Setup(r => r.GetAll(null)).Returns(new List<ClockApp>()); // vacío
        _clockAppRepository.Setup(r => r.Add(It.IsAny<ClockApp>()));

        var service = new ClockAppService(_clockAppRepository.Object);

        var past = DateTime.Now.AddMinutes(-45);
        var result = service.CalculateDifferenceInMinutes(past);

        result.Should().Be(-45);

        _clockAppRepository.Verify(r => r.GetAll(null), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    #endregion

    #region Now / constructor (crea si no existe, usa si existe)

    [TestMethod]
    public void Now_WhenClockExists_ShouldUseItsOffset_AndNotAdd()
    {
        var existing = new ClockApp { OffsetMinutes = 30 };
        _clockAppRepository.Setup(r => r.GetAll(null)).Returns(new List<ClockApp> { existing });

        var service = new ClockAppService(_clockAppRepository.Object);

        var result = service.Now();

        var expected = DateTime.Now.AddMinutes(30);
        (result - expected).TotalSeconds.Should().BeLessThan(1);

        _clockAppRepository.Verify(r => r.GetAll(null), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Never);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Now_WhenNoClockExists_ShouldCreateOneWithZeroOffset_AndReturnNow()
    {
        _clockAppRepository.Setup(r => r.GetAll(null)).Returns(new List<ClockApp>());
        _clockAppRepository.Setup(r => r.Add(It.IsAny<ClockApp>()));

        var service = new ClockAppService(_clockAppRepository.Object);

        var result = service.Now();

        var expected = DateTime.Now;
        (result - expected).TotalSeconds.Should().BeLessThan(1);

        _clockAppRepository.Verify(r => r.GetAll(null), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    #endregion

    #region Update

    [TestMethod]
    public void Get_WhenClockAppExists_ShouldReturnEntity()
    {
        // Arrange
        var expected = new ClockApp { OffsetMinutes = 20 };
        _clockAppRepository.Setup(r => r.Get(It.IsAny<Expression<Func<ClockApp, bool>>>()))
            .Returns(expected);

        // Act
        var result = _clockAppService.Get(c => c.OffsetMinutes == 20);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);

        _clockAppRepository.Verify(r => r.Get(It.IsAny<Expression<Func<ClockApp, bool>>>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Get_WhenClockAppDoesNotExist_ShouldReturnNull()
    {
        // Arrange
        _clockAppRepository.Setup(r => r.Get(It.IsAny<Expression<Func<ClockApp, bool>>>()))
            .Returns((ClockApp?)null);

        // Act
        var result = _clockAppService.Get(c => c.OffsetMinutes == 99);

        // Assert
        result.Should().BeNull();

        _clockAppRepository.Verify(r => r.Get(It.IsAny<Expression<Func<ClockApp, bool>>>()), Times.Once);
        _clockAppRepository.VerifyNoOtherCalls();
    }

    #endregion
}
