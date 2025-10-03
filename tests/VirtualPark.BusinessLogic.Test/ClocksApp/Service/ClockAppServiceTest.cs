using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.ClocksApp.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Models;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.ClocksApp.Service;

[TestClass]
[TestCategory("ClockAppService")]
public class ClockAppServiceTest
{
    private ClockAppService _clockAppService = null!;
    private Mock<IRepository<ClockApp>> _clockAppRepository = null!;
    private ClockAppArgs _clockArgs = null!;

    [TestInitialize]
    public void Initialize()
    {
        _clockAppRepository = new Mock<IRepository<ClockApp>>(MockBehavior.Strict);
        _clockAppService = new ClockAppService(_clockAppRepository.Object);
        _clockArgs = new ClockAppArgs("2025-10-02 18:30");
    }

    #region CalculateDifferenceInMinutes
    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenSameDateTime_ShouldReturnZero()
    {
        var now = DateTime.Now;

        var result = _clockAppService.CalculateDifferenceInMinutes(now);

        result.Should().Be(0);
    }

    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenFutureDateTime_ShouldReturnPositive()
    {
        var future = DateTime.Now.AddMinutes(30);

        var result = _clockAppService.CalculateDifferenceInMinutes(future);

        result.Should().Be(30);
    }

    [TestMethod]
    public void CalculateDifferenceInMinutes_WhenPastDateTime_ShouldReturnNegative()
    {
        var past = DateTime.Now.AddMinutes(-45);

        var result = _clockAppService.CalculateDifferenceInMinutes(past);

        result.Should().Be(-45);
    }

    #endregion

    #region GetOrCreate
    private static ClockApp InvokeGetOCreate(ClockAppService svc)
    {
        var mi = typeof(ClockAppService).GetMethod("GetOCreate",
            BindingFlags.NonPublic | BindingFlags.Instance);
        return (ClockApp)mi!.Invoke(svc, null)!;
    }

    [TestMethod]
    public void GetOCreate_WhenClockExists_ShouldReturnExistingAndNotAdd()
    {
        var existing = new ClockApp();

        _clockAppRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<ClockApp, bool>>?>()))
            .Returns(new List<ClockApp> { existing });

        var result = InvokeGetOCreate(_clockAppService);

        result.Should().BeSameAs(existing);

        _clockAppRepository.Verify(r => r.GetAll(It.IsAny<Expression<Func<ClockApp, bool>>?>()), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Never);
    }

    [TestMethod]
    public void GetOCreate_WhenNoClockExists_ShouldCreateNewAndPersist()
    {
        _clockAppRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<ClockApp, bool>>?>()))
            .Returns(new List<ClockApp>());

        ClockApp? added = null;
        _clockAppRepository
            .Setup(r => r.Add(It.IsAny<ClockApp>()))
            .Callback<ClockApp>(c => added = c);

        var result = InvokeGetOCreate(_clockAppService);

        result.Should().NotBeNull();
        added.Should().NotBeNull();
        result.Should().BeSameAs(added);

        _clockAppRepository.Verify(r => r.GetAll(It.IsAny<Expression<Func<ClockApp, bool>>?>()), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
    }

    #endregion

}
