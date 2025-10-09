using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.WebApi.Controllers.Strategy;
using VirtualPark.WebApi.Controllers.Strategy.ModelsIn;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Strategy;

[TestClass]
public class StrategyControllerTest
{
    private Mock<IStrategyService> _strategyServiceMock = null!;
    private Mock<IClockAppService> _mockClockService = null!;
    private StrategyController _strategyController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _strategyServiceMock = new Mock<IStrategyService>(MockBehavior.Strict);
        _strategyController = new StrategyController(_strategyServiceMock.Object);

        _mockClockService = new Mock<IClockAppService>();
        _mockClockService.Setup(x => x.Now()).Returns(new DateTime(2025, 10, 7, 12, 0, 0));
        ValidationServices.ClockService = _mockClockService.Object;
    }

    #region CreateActiveStrategy
    [TestMethod]
    public void CreateActiveStrategy_ValidInput_ReturnsCreatedStrategyResponse()
    {
        var returnId = Guid.NewGuid();

        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Combo",
            Date = "2025-10-08"
        };

        var expectedArgs = request.ToArgs();

        _strategyServiceMock
            .Setup(s => s.Create(It.Is<ActiveStrategyArgs>(a =>
                a.StrategyKey == expectedArgs.StrategyKey &&
                a.Date == expectedArgs.Date)))
            .Returns(returnId);

        var response = _strategyController.CreateActiveStrategy(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateActiveStrategyResponse>();
        response.Id.Should().Be(returnId.ToString());

        _strategyServiceMock.VerifyAll();
    }

    [TestMethod]
    public void CreateActiveStrategy_ShouldThrow_WhenDateFormatIsInvalid()
    {
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Attraction",
            Date = "08/10/2025" // Formato incorrecto
        };

        Action act = () => _strategyController.CreateActiveStrategy(request);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid date format*");

        _strategyServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CreateActiveStrategy_ShouldThrow_WhenDateIsInPast()
    {
        var request = new CreateActiveStrategyRequest
        {
            StrategyKey = "Event",
            Date = "2025-10-06" // Fecha pasada
        };

        Action act = () => _strategyController.CreateActiveStrategy(request);

        act.Should().Throw<ArgumentException>()
            .WithMessage("*Date cannot be in the past*");

        _strategyServiceMock.VerifyNoOtherCalls();
    }
    #endregion

    #region GetActiveStrategy
    [TestMethod]
    public void GetActiveStrategy_ValidDate_ReturnsStrategyResponse()
    {
        var date = "2025-10-08";
        var dateOnly = new DateOnly(2025, 10, 08);

        var strategyArgs = new ActiveStrategyArgs("Combo", date);

        _strategyServiceMock
            .Setup(s => s.Get(dateOnly))
            .Returns(strategyArgs);

        var result = _strategyController.GetActiveStrategy(date);

        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value!.Key.Should().Be("Combo");
        result.Value.Date.Should().Be("2025-10-08");

        _strategyServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetActiveStrategy_ShouldThrow_WhenDateFormatIsInvalid()
    {
        var invalidDate = "08/10/2025";

        Action act = () => _strategyController.GetActiveStrategy(invalidDate);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid date format*");

        _strategyServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetActiveStrategy_ShouldThrow_WhenServiceReturnsNull()
    {
        var date = "2025-10-08";
        var dateOnly = new DateOnly(2025, 10, 08);

        _strategyServiceMock
            .Setup(s => s.Get(dateOnly))
            .Returns((ActiveStrategyArgs?)null);

        Action act = () => _strategyController.GetActiveStrategy(date);

        act.Should().Throw<NullReferenceException>();

        _strategyServiceMock.VerifyAll();
    }
    #endregion

    #region GetActiveStrategies
    [TestMethod]
    public void GetActiveStrategies_ShouldReturnMappedList()
    {
        var strategies = new List<ActiveStrategyArgs>
        {
            new ActiveStrategyArgs("Attraction", "2025-10-08"),
            new ActiveStrategyArgs("Combo", "2025-10-09")
        };

        _strategyServiceMock
            .Setup(s => s.GetAll())
            .Returns(strategies);

        var result = _strategyController.GetActiveStrategies();

        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Should().HaveCount(2);

        var list = result.Value!.ToList();

        list[0].Key.Should().Be("Attraction");
        list[0].Date.Should().Be("2025-10-08");

        list[1].Key.Should().Be("Combo");
        list[1].Date.Should().Be("2025-10-09");

        _strategyServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetActiveStrategies_ShouldReturnEmptyList_WhenNoStrategiesExist()
    {
        _strategyServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var result = _strategyController.GetActiveStrategies();

        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.Should().BeEmpty();

        _strategyServiceMock.VerifyAll();
    }
    #endregion

    #region DeleteStrategy
    [TestMethod]
    public void DeleteStrategy_ShouldRemoveStrategy_WhenDateIsValid()
    {
        var date = "2025-10-08";
        var dateOnly = new DateOnly(2025, 10, 08);

        _strategyServiceMock
            .Setup(s => s.Remove(dateOnly))
            .Verifiable();

        var result = _strategyController.DeleteStrategy(date);

        result.Should().BeOfType<NoContentResult>();

        _strategyServiceMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteStrategy_ShouldThrow_WhenDateFormatIsInvalid()
    {
        var invalidDate = "not-a-date";

        Action act = () => _strategyController.DeleteStrategy(invalidDate);

        act.Should().Throw<ArgumentException>();

        _strategyServiceMock.VerifyNoOtherCalls();
    }
    #endregion

    #region UpdateStrategy
    [TestMethod]
    public void UpdateStrategy_ValidInput_ShouldCallServiceUpdate()
    {
        var date = "2025-10-08";
        var dateOnly = new DateOnly(2025, 10, 08);

        var request = new UpdateActiveStrategyRequest
        {
            Key = "Event"
        };

        var expectedArgs = request.ToArgs(dateOnly);

        _strategyServiceMock
            .Setup(s => s.Update(It.Is<ActiveStrategyArgs>(a =>
                    a.StrategyKey == expectedArgs.StrategyKey &&
                    a.Date == expectedArgs.Date),
                dateOnly))
            .Verifiable();

        var result = _strategyController.UpdateStrategy(request, date);

        result.Should().BeOfType<NoContentResult>();

        _strategyServiceMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateStrategy_ShouldThrow_WhenDateFormatIsInvalid()
    {
        var invalidDate = "invalid";
        var request = new UpdateActiveStrategyRequest
        {
            Key = "Combo"
        };

        Action act = () => _strategyController.UpdateStrategy(request, invalidDate);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid date format*");

        _strategyServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateStrategy_ShouldThrow_WhenServiceThrows()
    {
        var date = "2025-10-08";
        var dateOnly = new DateOnly(2025, 10, 08);

        var request = new UpdateActiveStrategyRequest
        {
            Key = "Event"
        };

        _strategyServiceMock
            .Setup(s => s.Update(It.IsAny<ActiveStrategyArgs>(), dateOnly))
            .Throws(new InvalidOperationException($"ActiveStrategy with date {dateOnly} not found."));

        Action act = () => _strategyController.UpdateStrategy(request, date);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"ActiveStrategy with date {dateOnly} not found.");

        _strategyServiceMock.VerifyAll();
    }
    #endregion
}
