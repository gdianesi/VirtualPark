using System.Linq.Expressions;
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

}
