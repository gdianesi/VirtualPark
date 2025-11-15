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
    private Mock<IRepository<ClockApp>> _clockAppRepository = null!;
    private ClockAppService _clockAppService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _clockAppRepository = new Mock<IRepository<ClockApp>>(MockBehavior.Strict);
        _clockAppService = new ClockAppService(_clockAppRepository.Object);
    }

    #region Create

    [TestMethod]
    public void Create_WhenArgsAreValid_ShouldPersistAndReturnId()
    {
        var expectedDate = new DateTime(2025, 10, 3, 12, 0, 0);

        var args = new ClockAppArgs("2025-10-03 12:00:00");

        ClockApp? captured = null;

        _clockAppRepository
            .Setup(r => r.Add(It.Is<ClockApp>(c => c.DateSystem == expectedDate)))
            .Callback<ClockApp>(c => captured = c);

        var id = _clockAppService.Create(args);

        captured.Should().NotBeNull();
        captured!.DateSystem.Should().Be(expectedDate);
        id.Should().Be(captured.Id);

        _clockAppRepository.Verify(
            r => r.Add(It.Is<ClockApp>(c => c.DateSystem == expectedDate)),
            Times.Once);
    }
    #endregion
    #region Get
    [TestMethod]
    public void Get_WhenNoClockInDb_ShouldReturnNewWithNow()
    {
        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([]);

        var before = DateTime.Now;

        var clock = _clockAppService.Get();

        var after = DateTime.Now;

        clock.Should().NotBeNull();
        clock.DateSystem.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);

        _clockAppRepository.Verify(r => r.GetAll(), Times.Once);
    }
    #endregion
    #region Now

    [TestMethod]
    public void Get_WhenNoClockInDb_ShouldReturnNewWithNow_AndNotPersist()
    {
        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([]);

        _clockAppService.Now().Should().BeOnOrBefore(DateTime.Now);
    }
    #endregion
    #region Update
    [TestMethod]
    public void Update_WhenNoClockExists_ShouldCreateNewClockApp()
    {
        var expectedDate = new DateTime(2025, 10, 03, 12, 00, 00);
        var args = new ClockAppArgs("2025-10-03 12:00:00");

        ClockApp? captured = null;

        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([]);

        _clockAppRepository
            .Setup(r => r.Add(It.IsAny<ClockApp>()))
            .Callback<ClockApp>(c => captured = c);

        _clockAppService.Update(args);

        captured.Should().NotBeNull();
        captured!.DateSystem.Should().Be(expectedDate);

        _clockAppRepository.Verify(r => r.GetAll(), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Once);
        _clockAppRepository.Verify(r => r.Update(It.IsAny<ClockApp>()), Times.Never);
    }

    [TestMethod]
    public void Update_WhenClockExists_ShouldUpdateExistingClockApp()
    {
        var clock = new ClockApp { DateSystem = new DateTime(2025, 10, 03, 12, 00, 00) };
        var args = new ClockAppArgs("2026-11-03 12:00:00");

        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([clock]);

        _clockAppRepository
            .Setup(r => r.Update(clock));

        _clockAppService.Update(args);

        clock.DateSystem.Should().Be(args.SystemDateTime);

        _clockAppRepository.Verify(r => r.GetAll(), Times.Once);
        _clockAppRepository.Verify(r => r.Update(clock), Times.Once);
        _clockAppRepository.Verify(r => r.Add(It.IsAny<ClockApp>()), Times.Never);
    }

    #endregion
    #region Remove
    [TestMethod]
    public void Remove_WhenClockAppExists_ShouldCallRepositoryRemove()
    {
        var existing = new ClockApp { DateSystem = new DateTime(2025, 10, 3, 12, 0, 0) };

        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([existing]);

        _clockAppRepository
            .Setup(r => r.Remove(existing));

        _clockAppService.Remove();

        _clockAppRepository.Verify(r => r.GetAll(), Times.Once);
        _clockAppRepository.Verify(r => r.Remove(existing), Times.Once);
    }

    [TestMethod]
    public void Remove_WhenNoClockAppExists_ShouldNotCallRepositoryRemove()
    {
        _clockAppRepository
            .Setup(r => r.GetAll())
            .Returns([]);

        _clockAppService.Remove();

        _clockAppRepository.Verify(r => r.GetAll(), Times.Once);
        _clockAppRepository.Verify(r => r.Remove(It.IsAny<ClockApp>()), Times.Never);
    }

    #endregion
}
