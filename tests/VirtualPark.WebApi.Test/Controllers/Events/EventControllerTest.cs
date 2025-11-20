using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.WebApi.Controllers.Events;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Events;

[TestClass]
public class EventControllerTest
{
    private Mock<IEventService> _eventServiceMock = null!;
    private EventController _eventController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _eventServiceMock = new Mock<IEventService>(MockBehavior.Strict);
        _eventController = new EventController(_eventServiceMock.Object);
    }
    #region Create
    [TestMethod]
    public void CreateEvent_WhenRequestIsNull_ShouldThrowArgumentNullException()
    {
        Action act = () => _eventController.CreateEvent(null!);

        act.Should()
            .Throw<ArgumentNullException>();

        _eventServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void CreateEvent_ValidInput_ReturnsCreatedEventResponse()
    {
        var attractionId = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateEventRequest
        {
            Name = "Halloween Party",
            Date = "2025-10-31",
            Capacity = "200",
            Cost = "1500",
            AttractionsIds = [attractionId]
        };

        var expectedArgs = request.ToArgs();

        _eventServiceMock
            .Setup(s => s.Create(It.Is<EventsArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Date == expectedArgs.Date &&
                a.Capacity == expectedArgs.Capacity &&
                a.Cost == expectedArgs.Cost &&
                a.AttractionIds.SequenceEqual(expectedArgs.AttractionIds))))
            .Returns(returnId);

        var response = _eventController.CreateEvent(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateEventResponse>();
        response.Id.Should().Be(returnId.ToString());

        _eventServiceMock.VerifyAll();
    }
    #endregion
    #region Get
    [TestMethod]
    public void GetEventById_WhenEventDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _eventServiceMock
            .Setup(s => s.Get(id))
            .Returns((Event?)null);

        Action act = () => _eventController.GetEventById(id.ToString());

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Event with id {id} not found.");

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetEventById_ValidInput_ReturnsGetEventResponse()
    {
        var attraction = new Attraction()
        {
            Name = "Roller Coaster",
            Capacity = 20,
            Available = true
        };
        var attractionId = attraction.Id.ToString();
        var ev = new Event
        {
            Name = "Halloween Party",
            Date = new DateTime(2025, 10, 31),
            Capacity = 200,
            Cost = 1500,
            Attractions = [attraction]
        };

        var id = ev.Id;

        _eventServiceMock
            .Setup(s => s.Get(id))
            .Returns(ev);

        var response = _eventController.GetEventById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetEventResponse>();
        response.Id.Should().Be(id.ToString());
        response.Name.Should().Be("Halloween Party");
        response.Date.Should().Be(ev.Date.ToString("yyyy-MM-dd"));
        response.Capacity.Should().Be(ev.Capacity.ToString());
        response.Cost.Should().Be(ev.Cost.ToString());
        response.Attractions.Should().ContainSingle().Which.Should().Be(attractionId);

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetEventById_ShouldReturnEvent_WithMultipleAttractions_AndDateFormatted()
    {
        var a1 = new Attraction { Name = "Roller Coaster", Capacity = 20, Available = true };
        var a2 = new Attraction { Name = "Haunted House", Capacity = 15, Available = true };

        var ev = new Event
        {
            Name = "Halloween Party",
            Date = new DateTime(2025, 10, 31, 18, 30, 00),
            Capacity = 200,
            Cost = 1500,
            Attractions = [a1, a2]
        };
        var id = ev.Id;

        _eventServiceMock.Setup(s => s.Get(id)).Returns(ev);

        var res = _eventController.GetEventById(id.ToString());

        res.Should().NotBeNull();
        res.Name.Should().Be("Halloween Party");
        res.Date.Should().Be("2025-10-31");
        res.Attractions.Should().HaveCount(2);
        res.Attractions.Should().Contain([a1.Id.ToString(), a2.Id.ToString()]);

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetEventById_ShouldIncludeTicketsSold()
    {
        var attraction = new Attraction { Name = "Roller", Capacity = 10, Available = true };

        var ev = new Event
        {
            Name = "Halloween",
            Date = new DateTime(2025, 10, 31),
            Capacity = 100,
            Cost = 200,
            Attractions = [attraction],
            Tickets =
            [
                new Ticket { Id = Guid.NewGuid() },
                new Ticket { Id = Guid.NewGuid() },
                new Ticket { Id = Guid.NewGuid() }
            ]
        };

        var id = ev.Id;

        _eventServiceMock.Setup(s => s.Get(id)).Returns(ev);

        var result = _eventController.GetEventById(id.ToString());

        result.TicketsSold.Should().Be("3");

        _eventServiceMock.VerifyAll();
    }

    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllEvents_WhenTicketsAreNull_ShouldMapTicketsSoldAsZero()
    {
        var ev = new Event
        {
            Name = "Test",
            Date = new DateTime(2025, 10, 31),
            Capacity = 100,
            Cost = 200,
            Attractions = [],
            Tickets = null
        };

        _eventServiceMock
            .Setup(s => s.GetAll())
            .Returns([ev]);

        var result = _eventController.GetAllEvents().First();

        result.TicketsSold.Should().Be("0");

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllEvents_ShouldReturnMappedList()
    {
        var attraction1 = new Attraction
        {
            Name = "Roller Coaster",
            Capacity = 20,
            Available = true
        };

        var attraction2 = new Attraction
        {
            Name = "Haunted House",
            Capacity = 15,
            Available = true
        };

        var event1 = new Event
        {
            Name = "Halloween Party",
            Date = new DateTime(2025, 10, 31),
            Capacity = 200,
            Cost = 1500,
            Attractions = [attraction1]
        };

        var event2 = new Event
        {
            Name = "Christmas Show",
            Date = new DateTime(2025, 12, 24),
            Capacity = 300,
            Cost = 2500,
            Attractions = [attraction2]
        };

        var events = new List<Event> { event1, event2 };

        _eventServiceMock
            .Setup(s => s.GetAll())
            .Returns(events);

        var result = _eventController.GetAllEvents();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Name.Should().Be("Halloween Party");
        first.Cost.Should().Be(event1.Cost.ToString());

        var second = result.Last();
        second.Name.Should().Be("Christmas Show");
        second.Capacity.Should().Be(event2.Capacity.ToString());

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllEvents_ShouldReturnEmptyList_WhenNoEventsExist()
    {
        _eventServiceMock.Setup(s => s.GetAll()).Returns([]);

        var result = _eventController.GetAllEvents();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllEvents_ShouldMapEvent_WithNoAttractions()
    {
        var ev = new Event
        {
            Name = "Solo Charla",
            Date = new DateTime(2025, 09, 01),
            Capacity = 50,
            Cost = 0,
            Attractions = []
        };

        _eventServiceMock.Setup(s => s.GetAll()).Returns([ev]);

        var result = _eventController.GetAllEvents();

        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Solo Charla");
        result[0].Date.Should().Be("2025-09-01");
        result[0].Attractions.Should().NotBeNull();
        result[0].Attractions.Should().BeEmpty();

        _eventServiceMock.VerifyAll();
    }
    #endregion

    #region Delete
    [TestMethod]
    public void DeleteEvent_ShouldRemoveEvent_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _eventServiceMock
            .Setup(s => s.Remove(id))
            .Verifiable();

        _eventController.DeleteEvent(id.ToString());

        _eventServiceMock.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void UpdateEvent_WhenRequestIsNull_ShouldThrowArgumentNullException()
    {
        var id = Guid.NewGuid();

        Action act = () => _eventController.UpdateEvent(null!, id.ToString());

        act.Should().Throw<ArgumentNullException>();
    }

    [TestMethod]
    public void UpdateEvent_ValidInput_ShouldCallServiceUpdate()
    {
        var id = Guid.NewGuid();
        var attractionId = Guid.NewGuid().ToString();

        var request = new CreateEventRequest
        {
            Name = "Updated Halloween",
            Date = "2025-11-01",
            Capacity = "250",
            Cost = "2000",
            AttractionsIds = [attractionId]
        };

        var expectedArgs = request.ToArgs();

        _eventServiceMock
            .Setup(s => s.Update(
                It.Is<EventsArgs>(a =>
                    a.Name == expectedArgs.Name &&
                    a.Date == expectedArgs.Date &&
                    a.Capacity == expectedArgs.Capacity &&
                    a.Cost == expectedArgs.Cost &&
                    a.AttractionIds.SequenceEqual(expectedArgs.AttractionIds)),
                id))
            .Verifiable();

        _eventController.UpdateEvent(request, id.ToString());

        _eventServiceMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateEvent_ShouldCallServiceUpdate_WithMultipleAttractions()
    {
        var eventId = Guid.NewGuid();
        var id1 = Guid.NewGuid().ToString();
        var id2 = Guid.NewGuid().ToString();

        var req = new CreateEventRequest
        {
            Name = "Evento Actualizado",
            Date = "2025-11-15",
            Capacity = "600",
            Cost = "4200",
            AttractionsIds = [id1, id2]
        };
        var expected = req.ToArgs();

        _eventServiceMock
            .Setup(s => s.Update(It.Is<EventsArgs>(a =>
                    a.Name == expected.Name &&
                    a.Date == expected.Date &&
                    a.Capacity == expected.Capacity &&
                    a.Cost == expected.Cost &&
                    a.AttractionIds.SequenceEqual(expected.AttractionIds)),
                eventId))
            .Verifiable();

        _eventController.UpdateEvent(req, eventId.ToString());

        _eventServiceMock.VerifyAll();
    }
    #endregion

}
