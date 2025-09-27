using System.Linq.Expressions;
using FluentAssertions;
using Moq;
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
    private Mock<IRepository<Event>> _repositoryMock = null!;
    private EventService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _repositoryMock = new Mock<IRepository<Event>>();
        _service = new EventService(_repositoryMock.Object);
    }

    #region Id
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_WhenArgsAreValid_ShouldReturnEventId()
    {
        var args = new EventsArgs("Halloween", "2025-12-30", 100, 500);

        var result = _service.Create(args);

        result.Should().NotBe(Guid.Empty);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Event>()), Times.Once);
    }
    #endregion

    #region Create
    [TestMethod]
    [TestCategory("Behaviour")]
    public void Create_ShouldCallRepositoryAddWithMappedEntity()
    {
        var args = new EventsArgs("Christmas Party", "2025-12-24", 200, 1000);

        Event? capturedEvent = null;
        _repositoryMock.Setup(r => r.Add(It.IsAny<Event>()))
            .Callback<Event>(e => capturedEvent = e);

        var id = _service.Create(args);

        capturedEvent.Should().NotBeNull();
        capturedEvent!.Id.Should().Be(id);
        capturedEvent.Name.Should().Be("Christmas Party");
        capturedEvent.Capacity.Should().Be(200);
        capturedEvent.Cost.Should().Be(1000);
    }
    #endregion

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

        _repositoryMock
            .Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns(ev);

        _service.Remove(ev.Id);

        _repositoryMock.Verify(r => r.Remove(ev), Times.Once);
    }
    #endregion
    #endregion
}
