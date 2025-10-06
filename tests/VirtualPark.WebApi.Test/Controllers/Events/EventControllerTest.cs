using Moq;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Events;

[TestClass]
public class EventControllerTest
{
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
}

