using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Strategy.Entity;
using VirtualPark.BusinessLogic.Strategy.Models;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.Reflection;
using VirtualPark.ReflectionAbstractions;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("ActiveStrategyService")]
public class ActiveStrategyServiceTest
{
    private Mock<IRepository<ActiveStrategy>> _repoMock = null!;
    private ActiveStrategyService _service = null!;
    private Mock<IStrategyFactory> _factoryMock = null!;
    private Mock<ILoadAssembly<IStrategy>> _loadAssemblyMock = null!;

    [TestInitialize]
    public void Initialize()
    {
        _loadAssemblyMock = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        _repoMock = new Mock<IRepository<ActiveStrategy>>(MockBehavior.Strict);
        _factoryMock = new Mock<IStrategyFactory>(MockBehavior.Strict);
        _service = new ActiveStrategyService(_repoMock.Object, _factoryMock.Object, _loadAssemblyMock.Object);
    }

    #region Create

    #region Success

    [TestMethod]
    public void Create_ShouldAdd_WhenDateDoesNotHaveActiveStrategy()
    {
        var args = new ActiveStrategyArgs("Combo", "2029-10-07");

        _factoryMock
            .Setup(f => f.Create(args.StrategyKey))
            .Returns(Mock.Of<IStrategy>());

        _repoMock
            .Setup(r => r.Get(a => a.Date == args.Date))
            .Returns((ActiveStrategy?)null);

        ActiveStrategy? captured = null;
        _repoMock
            .Setup(r => r.Add(It.Is<ActiveStrategy>(a =>
                a.StrategyKey == args.StrategyKey &&
                a.Date == args.Date)))
            .Callback<ActiveStrategy>(e => captured = e);

        var result = _service.Create(args);

        result.Should().NotBeEmpty();
        captured.Should().NotBeNull();
        captured!.Id.Should().Be(result);
        captured.StrategyKey.Should().Be("Combo");
        captured.Date.Should().Be(new DateOnly(2029, 10, 07));

        _repoMock.VerifyAll();
        _factoryMock.VerifyAll();
    }

    [TestMethod]
    public void Create_ShouldUpdate_WhenDateAlreadyHasActiveStrategy()
    {
        var args = new ActiveStrategyArgs("Event", "2029-10-07");
        var existing = new ActiveStrategy { StrategyKey = "Attraction", Date = args.Date };

        _factoryMock
            .Setup(f => f.Create(args.StrategyKey))
            .Returns(Mock.Of<IStrategy>());

        _repoMock
            .Setup(r => r.Get(a => a.Date == args.Date))
            .Returns(existing);

        _repoMock
            .Setup(r => r.Update(It.Is<ActiveStrategy>(a =>
                a.Id == existing.Id &&
                a.Date == args.Date &&
                a.StrategyKey == "Event")));

        var result = _service.Create(args);

        result.Should().Be(existing.Id);

        _repoMock.VerifyAll();
        _factoryMock.VerifyAll();
    }

    #endregion

    #region Failure

    [TestMethod]
    public void Create_ShouldThrow_WhenStrategyKeyIsInvalid()
    {
        var args = new ActiveStrategyArgs("InvalidStrategy", "2029-10-07");

        _factoryMock
            .Setup(f => f.Create(args.StrategyKey))
            .Throws(new KeyNotFoundException($"Estrategia desconocida '{args.StrategyKey}'."));

        var act = () => _service.Create(args);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"The strategy '{args.StrategyKey}' is not valid.");

        _factoryMock.VerifyAll();
        _repoMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region Get

    #region Success

    [TestMethod]
    public void Get_ShouldReturnActiveStrategyArgs_WhenExistsForDate()
    {
        var date = new DateOnly(2025, 10, 08);
        var existing = new ActiveStrategy { StrategyKey = "Combo", Date = date };

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns(existing);

        var result = _service.Get(date);

        result.Should().NotBeNull();
        result!.StrategyKey.Should().Be("Combo");
        result.Date.Should().Be(date);

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Get_ShouldReturnNull_WhenNotExistsForDate()
    {
        var date = new DateOnly(2025, 10, 08);

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns((ActiveStrategy?)null);

        var result = _service.Get(date);

        result.Should().BeNull();

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region GetAll

    #region Success

    [TestMethod]
    public void GetAllStrategies_ShouldReturnListOfStrategyArgs_WhenRepositoryHasData()
    {
        var expectedKeys = new List<string> { "Attraction", "Combo", "Event" };

        _loadAssemblyMock.Setup(l => l.GetImplementationKeys())
            .Returns([]);

        var result = _service.GetAllStrategies();

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
        result.Select(r => r.Key).Should().BeEquivalentTo(expectedKeys);

        _loadAssemblyMock.Verify(l => l.GetImplementationKeys(), Times.Once);
        _loadAssemblyMock.VerifyNoOtherCalls();
        _repoMock.VerifyNoOtherCalls();
        _factoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetAll_ShouldReturnEmptyList_WhenRepositoryReturnsEmptyList()
    {
        _repoMock
            .Setup(r => r.GetAll())
            .Returns([]);

        var result = _service.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _repoMock.Verify(r => r.GetAll(), Times.Once);
        _repoMock.VerifyNoOtherCalls();
        _loadAssemblyMock.VerifyNoOtherCalls();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region Update

    #region Success

    [TestMethod]
    public void Update_ShouldApplyArgsAndUpdate_WhenEntityExistsForDate()
    {
        var date = new DateOnly(2025, 10, 08);
        var existing = new ActiveStrategy { StrategyKey = "Attraction", Date = date };
        var args = new ActiveStrategyArgs("Event", "2025-10-08");

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns(existing);

        _repoMock
            .Setup(r => r.Update(It.Is<ActiveStrategy>(a =>
                a.Id == existing.Id &&
                a.StrategyKey == "Event" &&
                a.Date == date)));

        _service.Update(args, date);

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Failure

    [TestMethod]
    public void Update_ShouldThrow_WhenEntityDoesNotExistForDate()
    {
        var date = new DateOnly(2025, 10, 08);
        var args = new ActiveStrategyArgs("Combo", "2025-10-08");

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns((ActiveStrategy?)null);

        var act = () => _service.Update(args, date);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"ActiveStrategy with date {date} not found.");

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region Remove

    #region Success

    [TestMethod]
    public void Remove_ShouldDelete_WhenEntityExistsForDate()
    {
        var date = new DateOnly(2025, 10, 08);
        var existing = new ActiveStrategy { StrategyKey = "Combo", Date = date };

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns(existing);

        _repoMock
            .Setup(r => r.Remove(It.Is<ActiveStrategy>(a => a == existing)));

        _service.Remove(date);

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #region Failure

    [TestMethod]
    public void Remove_ShouldThrow_WhenEntityDoesNotExistForDate()
    {
        var date = new DateOnly(2025, 10, 08);

        _repoMock
            .Setup(r => r.Get(a => a.Date == date))
            .Returns((ActiveStrategy?)null);

        var act = () => _service.Remove(date);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"ActiveStrategy with date {date} not found.");

        _repoMock.VerifyAll();
        _factoryMock.VerifyNoOtherCalls();
    }

    #endregion

    #endregion

    #region GetAllStrategies
    [TestMethod]
    public void GetAllStrategies_ShouldReturnOnlyHardcoded_WhenLoaderReturnsEmpty()
    {
        var loader = new Mock<ILoadAssembly<IStrategy>>(MockBehavior.Strict);
        loader.Setup(l => l.GetImplementationKeys()).Returns([]);

        var repo = new Mock<IRepository<ActiveStrategy>>(MockBehavior.Strict);
        var factory = new Mock<IStrategyFactory>(MockBehavior.Strict);

        var service = new ActiveStrategyService(repo.Object, factory.Object, loader.Object);

        var result = service.GetAllStrategies();

        result.Should().NotBeNull();
        result.Select(x => x.Key)
            .Should()
            .BeEquivalentTo(["Attraction", "Combo", "Event"], opts => opts.WithoutStrictOrdering());

        loader.Verify(l => l.GetImplementationKeys(), Times.Once);
        repo.VerifyNoOtherCalls();
        factory.VerifyNoOtherCalls();
        loader.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetAllStrategies_ShouldRemoveDuplicates_WhenPluginMatchesHardcoded()
    {
        _loadAssemblyMock.Setup(l => l.GetImplementationKeys())
            .Returns(["Combo"]);

        var result = _service.GetAllStrategies();

        result.Select(r => r.Key).Should().BeEquivalentTo(
            ["Attraction", "Combo", "Event"],
            opts => opts.WithoutStrictOrdering());

        _loadAssemblyMock.Verify(l => l.GetImplementationKeys(), Times.Once);
        _loadAssemblyMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetAllStrategies_ShouldNotThrow_WhenLoaderProvidesInvalidKeys_BecauseTheyAreFilteredBeforeCtor()
    {
        _loadAssemblyMock.Setup(l => l.GetImplementationKeys())
            .Returns([string.Empty, "  ", null, "WeekendBonus"]);

        var act = _service.GetAllStrategies;
        act.Should().NotThrow();

        var result = act.Invoke();

        result.Select(r => r.Key).Should().BeEquivalentTo(
            ["Attraction", "Combo", "Event", "WeekendBonus"],
            opts => opts.WithoutStrictOrdering());

        _loadAssemblyMock.Verify(l => l.GetImplementationKeys());
        _loadAssemblyMock.VerifyNoOtherCalls();
    }
    #endregion
}
