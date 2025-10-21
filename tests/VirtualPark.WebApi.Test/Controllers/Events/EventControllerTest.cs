using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
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
    #endregion

    #region GetAll
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
    #endregion

}
