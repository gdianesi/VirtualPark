using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Events.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetEventResponse")]
public class GetEventResponseTest
{
    private static Event BuildEntity(
        Guid? id = null,
        string? name = null,
        DateTime? date = null,
        int? capacity = null,
        int? cost = null,
        List<Guid>? attractionIds = null,
        int? ticketsSold = null)
    {
        var e = new Event
        {
            Id = id ?? Guid.NewGuid(),
            Name = name ?? "Halloween",
            Date = date ?? DateTime.Today,
            Capacity = capacity ?? 200,
            Cost = cost ?? 1500,
            Attractions = (attractionIds ?? [])
                .Select(g => new Attraction { Id = g })
                .ToList(),
            Tickets = Enumerable.Range(0, ticketsSold ?? 0)
                .Select(_ => new Ticket { Id = Guid.NewGuid() })
                .ToList()
        };

        return e;
    }

    #region Id
    [TestMethod]
    public void Id_ShouldMapCorrectly()
    {
        var id = Guid.NewGuid();
        var entity = BuildEntity(id: id);

        var dto = new GetEventResponse(entity);

        dto.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Name
    [TestMethod]
    public void Name_ShouldMapCorrectly()
    {
        var entity = BuildEntity(name: "Halloween Party");

        var dto = new GetEventResponse(entity);

        dto.Name.Should().Be("Halloween Party");
    }
    #endregion

    #region Date
    [TestMethod]
    public void Date_ShouldMapCorrectly()
    {
        var date = new DateTime?(DateTime.Today);
        var entity = BuildEntity(date: date);

        var dto = new GetEventResponse(entity);

        dto.Date.Should().Be(DateTime.Today.ToString("yyyy-MM-dd"));
    }
    #endregion

    #region Capacity
    [TestMethod]
    public void Capacity_ShouldMapCorrectly()
    {
        var entity = BuildEntity(capacity: 350);

        var dto = new GetEventResponse(entity);

        dto.Capacity.Should().Be("350");
    }
    #endregion

    #region Cost
    [TestMethod]
    public void Cost_ShouldMapCorrectly()
    {
        var entity = BuildEntity(cost: 999);

        var dto = new GetEventResponse(entity);

        dto.Cost.Should().Be("999");
    }
    #endregion

    #region Attractions
    [TestMethod]
    public void Attractions_ShouldMapCorrectly()
    {
        var a1 = Guid.NewGuid();
        var a2 = Guid.NewGuid();

        var entity = BuildEntity(attractionIds: [a1, a2]);

        var dto = new GetEventResponse(entity);

        dto.Attractions.Should().BeEquivalentTo([a1.ToString(), a2.ToString()]);
    }
    #endregion

    #region TicketsSold
    [TestMethod]
    public void TicketsSold_ShouldMapCorrectly()
    {
        var entity = BuildEntity(ticketsSold: 7);

        var dto = new GetEventResponse(entity);

        dto.TicketsSold.Should().Be("7");
    }
    #endregion
}
