using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Events.Models;
using VirtualPark.BusinessLogic.Events.Services;
using VirtualPark.Repository;

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
        var attractions = new List<Attraction> { new Attraction { Id = Guid.NewGuid(), Name = "Roller" } };

        var args = new EventsArgs("Halloween", "2025-12-30", 100, 500, attractions);

        var result = _service.Create(args);

        result.Should().NotBe(Guid.Empty);
        _repositoryMock.Verify(r => r.Add(It.IsAny<Event>()), Times.Once);
    }
    #endregion
}
