using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Events.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("Event")]
public sealed class EventServiceTest
{
    private Mock<IRepository<Event>> _eventRepositoryMock = null!;
    private EventService _eventService = null!;
    private Mock<IRepository<Attraction>> _attractionRepositoryMock = null!;
    private AttractionService _attractionService = null!;

    [TestInitialize]
    public void Setup()
    {
        _eventRepositoryMock = new Mock<IRepository<Event>>();
        _attractionRepositoryMock = new Mock<IRepository<Attraction>>();
        _attractionService = new AttractionService(_attractionRepositoryMock.Object);
        _eventService = new EventService(_eventRepositoryMock.Object, _attractionService);
    }

    #region Id
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldReturnEventId()
    {
        var attractionId = Guid.NewGuid();
        var attractions = new List<string> { attractionId.ToString() };
        var args = new EventsArgs("Halloween", "2025-12-30", 100, 500, attractions);

        var attraction = new Attraction { Id = attractionId, Name = "Roller Coaster" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(attraction);
        var result = _eventService.Create(args);

        result.Should().NotBe(Guid.Empty);
        _eventRepositoryMock.Verify(r => r.Add(It.IsAny<Event>()), Times.Once);
    }
    #endregion

    #region Create
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_ShouldCallRepositoryAddWithMappedEntity()
    {
        var attractionId = Guid.NewGuid();
        var attractions = new List<string> { attractionId.ToString() };
        var args = new EventsArgs("Christmas Party", "2025-12-24", 200, 1000, attractions);

        var attraction = new Attraction { Id = attractionId, Name = "Roller Coaster" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(attraction);

        Event? capturedEvent = null;
        _eventRepositoryMock.Setup(r => r.Add(It.IsAny<Event>()))
            .Callback<Event>(e => capturedEvent = e);

        var id = _eventService.Create(args);

        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(id);
        capturedEvent.Name.Should().Be("Christmas Party");
        capturedEvent.Capacity.Should().Be(200);
        capturedEvent.Cost.Should().Be(1000);
        capturedEvent.Attractions.Should().Contain(attraction);
    }

    #endregion
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Get_WhenEventExists_ShouldReturnEvent()
    {
        var eventId = Guid.NewGuid();
        var ev = new Event { Id = eventId, Name = "New Year Party" };

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        var result = _eventService.Get(e => e.Id == eventId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(eventId);
        result.Name.Should().Be("New Year Party");
    }
    #region Null
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Get_WhenEventDoesNotExist_ShouldReturnNull()
    {
        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns((Event?)null);

        var result = _eventService.Get(e => e.Id == Guid.NewGuid());

        result.Should().BeNull();
    }
    #endregion
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenEventsExist_ShouldReturnList()
    {
        var ev1 = new Event { Id = Guid.NewGuid(), Name = "Halloween" };
        var ev2 = new Event { Id = Guid.NewGuid(), Name = "Christmas" };
        var events = new List<Event> { ev1, ev2 };

        _eventRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(events);

        var result = _eventService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(ev1);
        result.Should().Contain(ev2);
    }
    #endregion
    #region EmptyList
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenNoEventsExist_ShouldReturnEmptyList()
    {
        _eventRepositoryMock
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns([]);

        var result = _eventService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
    #endregion
    #endregion

    #region Remove
    #region  Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenEventExists_ShouldRemoveFromRepository()
    {
        var eventId = Guid.NewGuid();
        var ev = new Event { Id = eventId, Name = "Summer Fest" };

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _eventService.Remove(eventId);

        _eventRepositoryMock.Verify(r => r.Remove(ev), Times.Once);
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenEventDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var eventId = Guid.NewGuid();

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns((Event?)null);

        Action act = () => _eventService.Remove(eventId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Event with id {eventId} not found.");
    }
    #endregion
    #endregion

    #region Update
    #region Success
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenEventExists_ShouldApplyChangesAndPersist()
    {
        var attractionOld = new Attraction { Name = "Ferris Road" };

        var existing = new Event
        {
            Name = "Old Name",
            Date = new DateTime(2025, 12, 1),
            Capacity = 50,
            Cost = 200,
            Attractions = [attractionOld]
        };

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(existing);

        var attractionId = Guid.NewGuid();
        var args = new EventsArgs("New Year Party", "2025-12-31", 100, 500, [attractionId.ToString()]);

        var attraction = new Attraction { Id = attractionId, Name = "Ferris Wheel" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(attraction);

        _eventService.Update(args, existing.Id);

        existing.Name.Should().Be("New Year Party");
        existing.Date.Should().Be(new DateTime(2025, 12, 31));
        existing.Capacity.Should().Be(100);
        existing.Cost.Should().Be(500);
        existing.Attractions.Should().Contain(attraction);

        _eventRepositoryMock.Verify(r => r.Update(existing), Times.Once);
    }
    #endregion
    #region Failure
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenEventDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var eventId = Guid.NewGuid();

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns((Event?)null);

        var args = new EventsArgs("Party", "2025-12-31", 100, 500, [Guid.NewGuid().ToString()]);

        Action act = () => _eventService.Update(args, eventId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Event with id {eventId} not found.");
    }
    #endregion
    #endregion

    #region Exist
    #region True
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Exist_WhenEventMatchesPredicate_ShouldReturnTrue()
    {
        _eventRepositoryMock
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(true);

        var result = _eventService.Exist(e => e.Name == "Halloween");

        result.Should().BeTrue();
    }
    #endregion

    #region False
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Exist_WhenNoEventMatchesPredicate_ShouldReturnFalse()
    {
        _eventRepositoryMock
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(false);

        var result = _eventService.Exist(e => e.Name == "NotExists");

        result.Should().BeFalse();
    }
    #endregion
    #endregion
}
