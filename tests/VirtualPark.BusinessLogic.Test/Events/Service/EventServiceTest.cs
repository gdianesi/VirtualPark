using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Events.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("Event")]
public sealed class EventServiceTest
{
    private Mock<IRepository<Attraction>> _attractionRepositoryMock = null!;
    private Mock<IRepository<Event>> _eventRepositoryMock = null!;
    private EventService _eventService = null!;

    [TestInitialize]
    public void Setup()
    {
        _eventRepositoryMock = new Mock<IRepository<Event>>();
        _attractionRepositoryMock = new Mock<IRepository<Attraction>>();
        _eventService = new EventService(_eventRepositoryMock.Object, _attractionRepositoryMock.Object);
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
        Guid result = _eventService.Create(args);

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

        Guid id = _eventService.Create(args);

        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(id);
        capturedEvent.Name.Should().Be("Christmas Party");
        capturedEvent.Capacity.Should().Be(200);
        capturedEvent.Cost.Should().Be(1000);
        capturedEvent.Attractions.Should().Contain(attraction);
    }

    [TestMethod]
    public void Create_MapsSingleAttractionId_WhenExists()
    {
        var id = Guid.NewGuid();
        var args = new EventsArgs("E", "2025-12-31", 1, 1, [id.ToString()]);
        var attraction = new Attraction { Id = id, Name = "A" };

        _attractionRepositoryMock.Setup(r => r.Get(a => a.Id == id)).Returns(attraction);

        _eventRepositoryMock.Setup(r => r.Add(It.Is<Event>(e =>
            e.Attractions.Count == 1 &&
            e.Attractions[0].Id == id)));

        Guid newId = _eventService.Create(args);

        newId.Should().NotBeEmpty();
        _attractionRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
    }

    [TestMethod]
    public void Create_Throws_WhenAttractionIdNotFound()
    {
        var missing = Guid.NewGuid();
        var args = new EventsArgs("E", "2025-12-31", 1, 1, [missing.ToString()]);

        _attractionRepositoryMock.Setup(r => r.Get(a => a.Id == missing)).Returns((Attraction?)null);

        Action act = () => _eventService.Create(args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Attraction with id {missing} not found.");

        _attractionRepositoryMock.VerifyAll();
        _eventRepositoryMock.VerifyAll();
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
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        Event? result = _eventService.Get(eventId);

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

        Event? result = _eventService.Get(Guid.NewGuid());

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
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(events);

        List<Event> result = _eventService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(ev1);
        result.Should().Contain(ev2);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_ShouldIncludeTickets()
    {
        var ev1 = new Event { Id = Guid.NewGuid(), Tickets = [new Ticket { Id = Guid.NewGuid() }] };
        var ev2 = new Event { Id = Guid.NewGuid(), Tickets = [] };

        _eventRepositoryMock
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns([ev1, ev2]);

        var result = _eventService.GetAll();

        result.Should().HaveCount(2);
        result[0].Tickets.Should().HaveCount(1);
        result[1].Tickets.Should().BeEmpty();

        _eventRepositoryMock.Verify(r =>
                r.GetAll(
                    It.IsAny<Expression<Func<Event, bool>>>(),
                    It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()),
            Times.Once);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _eventRepositoryMock
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns((List<Event>?)null);

        Action act = () => _eventService.GetAll();

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Do not have any events");

        _eventRepositoryMock.VerifyAll();
    }

    #endregion

    #region EmptyList

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenNoEventsExist_ShouldReturnEmptyList()
    {
        _eventRepositoryMock
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns([]);

        List<Event> result = _eventService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    #endregion

    #endregion

    #region Remove

    #region Success

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenEventExists_ShouldRemoveFromRepository()
    {
        var eventId = Guid.NewGuid();
        var ev = new Event
        {
            Id = eventId,
            Name = "Summer Fest",
            Attractions = [new Attraction { Id = Guid.NewGuid(), Name = "A1" }]
        };

        _eventRepositoryMock
            .Setup(r => r.Get(
                e => e.Id == eventId,
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        _eventRepositoryMock
            .Setup(r => r.Update(ev));

        _eventRepositoryMock
            .Setup(r => r.Remove(ev));

        _eventService.Remove(eventId);

        ev.Attractions.Should().BeEmpty();

        _eventRepositoryMock.VerifyAll();
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
            .Setup(r => r.Get(It.Is<Expression<Func<Attraction, bool>>>(expr => expr.Compile().Invoke(attraction))))
            .Returns(attraction);

        _eventRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(existing);

        _eventService.Update(args, existing.Id);

        existing.Name.Should().Be("New Year Party");
        existing.Date.Should().Be(new DateTime(2025, 12, 31));
        existing.Capacity.Should().Be(100);
        existing.Cost.Should().Be(500);
        existing.Attractions.Should().Contain(attraction);

        _eventRepositoryMock.Verify(r => r.Update(existing), Times.Once);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenEventExists_ShouldClearOldAttractionsBeforeAddingNew()
    {
        var oldAttraction = new Attraction { Id = Guid.NewGuid(), Name = "Old Ride" };
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Summer Fest",
            Attractions = [oldAttraction]
        };

        var newAttractionId = Guid.NewGuid();
        var args = new EventsArgs("Updated Fest", "2025-12-25", 300, 1500, [newAttractionId.ToString()]);

        var newAttraction = new Attraction { Id = newAttractionId, Name = "New Roller Coaster" };

        _eventRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        _attractionRepositoryMock
            .Setup(r => r.Get(It.Is<Expression<Func<Attraction, bool>>>(expr => expr.Compile().Invoke(newAttraction))))
            .Returns(newAttraction);

        _eventRepositoryMock.Setup(r => r.Update(ev));

        _eventService.Update(args, ev.Id);

        ev.Name.Should().Be("Updated Fest");
        ev.Capacity.Should().Be(300);
        ev.Cost.Should().Be(1500);

        ev.Attractions.Should().HaveCount(1);
        ev.Attractions.Should().Contain(newAttraction);
        ev.Attractions.Should().NotContain(oldAttraction);

        _eventRepositoryMock.Verify(r => r.Update(ev), Times.Once);
    }

    #endregion

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenArgsHaveNoAttractions_ShouldClearAllExistingOnes()
    {
        var oldA1 = new Attraction { Id = Guid.NewGuid(), Name = "A1" };
        var oldA2 = new Attraction { Id = Guid.NewGuid(), Name = "A2" };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Festival",
            Attractions = [oldA1, oldA2]
        };

        var args = new EventsArgs("Festival Updated", "2025-11-15", 150, 700, []);

        _eventRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        _eventRepositoryMock.Setup(r => r.Update(ev));

        _eventService.Update(args, ev.Id);

        ev.Name.Should().Be("Festival Updated");
        ev.Attractions.Should().BeEmpty();

        _eventRepositoryMock.Verify(r => r.Update(ev), Times.Once);
    }

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
    [TestCategory("Validation")]
    public void Exist_WhenEventWithGivenIdExists_ShouldReturnTrue()
    {
        var eventId = Guid.NewGuid();

        _eventRepositoryMock
            .Setup(r => r.Exist(It.Is<Expression<Func<Event, bool>>>(expr =>
                expr.Compile().Invoke(new Event { Id = eventId }))))
            .Returns(true);

        var result = _eventService.Exist(eventId);

        result.Should().BeTrue();

        _eventRepositoryMock.Verify(r => r.Exist(It.IsAny<Expression<Func<Event, bool>>>()), Times.Once);
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

        var result = _eventService.Exist(Guid.NewGuid());

        result.Should().BeFalse();
    }

    #endregion

    #endregion
    #region Get
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Get_ShouldIncludeTickets()
    {
        var eventId = Guid.NewGuid();

        var ev = new Event
        {
            Id = eventId,
            Name = "Test Event",
            Tickets = [new Ticket { Id = Guid.NewGuid() }]
        };

        _eventRepositoryMock
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Event, bool>>>(),
                It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()))
            .Returns(ev);

        var result = _eventService.Get(eventId);

        result.Should().NotBeNull();
        result!.Tickets.Should().NotBeNull();
        result.Tickets.Should().HaveCount(1);

        _eventRepositoryMock.Verify(r =>
                r.Get(
                    It.IsAny<Expression<Func<Event, bool>>>(),
                    It.IsAny<Func<IQueryable<Event>, IIncludableQueryable<Event, object>>>()),
            Times.Once);
    }
    #endregion
}
