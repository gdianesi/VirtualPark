using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
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
    private Mock<IRepository<Attraction>> _attractionRepositoryMock = null!;
    private Mock<IRepository<Event>> _eventRepositoryMock = null!;
    private EventService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _eventRepositoryMock = new Mock<IRepository<Event>>();
        _attractionRepositoryMock = new Mock<IRepository<Attraction>>();

        _service = new EventService(
            _eventRepositoryMock.Object,
            _attractionRepositoryMock.Object);
    }

    #region Id

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldReturnEventId()
    {
        List<Guid> attractions = [Guid.NewGuid(), Guid.NewGuid()];

        var attraction1 = new Attraction { Id = attractions[0], Name = "Roller Coaster" };
        var attraction2 = new Attraction { Id = attractions[1], Name = "Ferris Wheel" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns<Expression<Func<Attraction, bool>>>(expr =>
            {
                if(expr.Compile().Invoke(attraction1))
                {
                    return attraction1;
                }

                if(expr.Compile().Invoke(attraction2))
                {
                    return attraction2;
                }

                return null!;
            });

        var args = new EventsArgs("Christmas Party", "2025-12-24", 200, 1000, attractions);

        Guid result = _service.Create(args);

        result.Should().NotBe(Guid.Empty);
        _eventRepositoryMock.Verify(r => r.Add(It.IsAny<Event>()), Times.Once);
    }

    #endregion

    #region Create

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_ShouldCallRepositoryAddWithMappedEntity()
    {
        List<Guid> attractions = [Guid.NewGuid(), Guid.NewGuid()];

        var attraction1 = new Attraction { Id = attractions[0], Name = "Roller Coaster" };
        var attraction2 = new Attraction { Id = attractions[1], Name = "Ferris Wheel" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns<Expression<Func<Attraction, bool>>>(expr =>
            {
                if(expr.Compile().Invoke(attraction1))
                {
                    return attraction1;
                }

                if(expr.Compile().Invoke(attraction2))
                {
                    return attraction2;
                }

                return null!;
            });

        var args = new EventsArgs("Christmas Party", "2025-12-24", 200, 1000, attractions);

        Event? capturedEvent = null;
        _eventRepositoryMock.Setup(r => r.Add(It.IsAny<Event>()))
            .Callback<Event>(e => capturedEvent = e);

        Guid id = _service.Create(args);

        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(id);
        capturedEvent.Name.Should().Be("Christmas Party");
        capturedEvent.Capacity.Should().Be(200);
        capturedEvent.Cost.Should().Be(1000);
    }

    #endregion

    #region GetAll

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAll_WhenEventsExist_ShouldReturnAllEvents()
    {
        var ev1 = new Event { Id = Guid.NewGuid(), Name = "Halloween" };
        var ev2 = new Event { Id = Guid.NewGuid(), Name = "Christmas" };
        var events = new List<Event> { ev1, ev2 };

        _eventRepositoryMock
            .Setup(r => r.GetAll(null))
            .Returns(events);

        List<Event> result = _service.GetAll();

        result.Should().HaveCount(2);
        result.Should().Contain(ev1);
        result.Should().Contain(ev2);
    }

    #endregion

    #region Update

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Update_WhenEventExists_ShouldApplyChangesAndPersist()
    {
        var id = Guid.NewGuid();
        var existing = new Event
        {
            Id = id,
            Name = "Old Name",
            Date = new DateTime(2025, 12, 1),
            Capacity = 50,
            Cost = 200,
            Attractions = []
        };

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(existing);

        var attractionId1 = Guid.NewGuid();
        var attractionId2 = Guid.NewGuid();
        var attraction1 = new Attraction { Id = attractionId1, Name = "Roller Coaster" };
        var attraction2 = new Attraction { Id = attractionId2, Name = "Ferris Wheel" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns<Expression<Func<Attraction, bool>>>(expr =>
            {
                if(expr.Compile().Invoke(attraction1))
                {
                    return attraction1;
                }

                if(expr.Compile().Invoke(attraction2))
                {
                    return attraction2;
                }

                return null!;
            });

        var args = new EventsArgs(
            "New Name",
            "2026-01-10",
            300,
            1500,
            [attractionId1, attractionId2]);

        Event? captured = null;
        _eventRepositoryMock
            .Setup(r => r.Update(It.IsAny<Event>()))
            .Callback<Event>(e => captured = e);

        _service.Update(args, id);

        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);
        captured.Name.Should().Be("New Name");
        captured.Date.Should().Be(new DateTime(2026, 1, 10));
        captured.Capacity.Should().Be(300);
        captured.Cost.Should().Be(1500);
        captured.Attractions.Should().HaveCount(2);
        captured.Attractions.Should().Contain(a => a.Id == attractionId1);
        captured.Attractions.Should().Contain(a => a.Id == attractionId2);

        _eventRepositoryMock.Verify(r => r.Update(It.IsAny<Event>()), Times.Once);
    }

    #endregion

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenAttractionsExist_ShouldAssignThemToEvent()
    {
        var attractionId1 = Guid.NewGuid();
        var attractionId2 = Guid.NewGuid();
        var args = new EventsArgs("Concert", "2025-12-31", 500, 2000, [attractionId1, attractionId2]);

        var attraction1 = new Attraction { Id = attractionId1, Name = "Roller Coaster" };
        var attraction2 = new Attraction { Id = attractionId2, Name = "Ferris Wheel" };

        _attractionRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns<Expression<Func<Attraction, bool>>>(expr =>
            {
                if(expr.Compile().Invoke(attraction1))
                {
                    return attraction1;
                }

                if(expr.Compile().Invoke(attraction2))
                {
                    return attraction2;
                }

                return null!;
            });

        Event? capturedEvent = null;
        _eventRepositoryMock
            .Setup(r => r.Add(It.IsAny<Event>()))
            .Callback<Event>(e => capturedEvent = e);

        Guid id = _service.Create(args);

        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(id);
        capturedEvent.Attractions.Should().HaveCount(2);
        capturedEvent.Attractions.Should().Contain(a => a.Id == attractionId1);
        capturedEvent.Attractions.Should().Contain(a => a.Id == attractionId2);
    }

    #region Delete

    #region Success

    [TestMethod]
    public void Delete_WhenEventExists_ShouldRemoveEventFromRepository()
    {
        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Old Event",
            Date = DateTime.UtcNow.AddDays(10),
            Capacity = 100,
            Cost = 200
        };

        _eventRepositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _service.Remove(ev.Id);

        _eventRepositoryMock.Verify(r => r.Remove(ev), Times.Once);
    }

    #endregion

    #region Failure

    [TestMethod]
    [TestCategory("Behaviour")]
    public void Remove_WhenEventDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();
        _eventRepositoryMock.Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns((Event?)null);

        Action act = () => _service.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Event with id {id} not found.");
    }

    #endregion

    #endregion
}
