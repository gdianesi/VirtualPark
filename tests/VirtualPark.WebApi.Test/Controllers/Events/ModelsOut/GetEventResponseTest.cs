using FluentAssertions;
using VirtualPark.WebApi.Controllers.Events.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Events.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetEventResponse")]
public class GetEventResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        var id = Guid.NewGuid().ToString();
        var response = new GetEventResponse(
            id, "Halloween", "2025-12-01", "200", "1500", attractions);

        response.Id.Should().Be(id);
    }
    #endregion

    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        var response = new GetEventResponse(
            Guid.NewGuid().ToString(),
            "Halloween Party", "2025-12-01", "200", "1500", attractions);

        response.Name.Should().Be("Halloween Party");
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        const string date = "2025-12-01";
        var response = new GetEventResponse(
            Guid.NewGuid().ToString(),
            "Halloween Party",
            date, "200", "1500", attractions);

        response.Date.Should().Be(date);
    }
    #endregion

    #region Capacity
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        var response = new GetEventResponse(
            Guid.NewGuid().ToString(),
            "Halloween Party",
            "2025-12-01",
            "200", "1500", attractions);

        response.Capacity.Should().Be("200");
    }
    #endregion

    #region Cost
    [TestMethod]
    [TestCategory("Validation")]
    public void Cost_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        var response = new GetEventResponse(
            Guid.NewGuid().ToString(),
            "Halloween Party",
            "2025-12-01",
            "200",
            "1500", attractions);

        response.Cost.Should().Be("1500");
    }
    #endregion

    #region Attractions
    [TestMethod]
    [TestCategory("Validation")]
    public void Attractions_Getter_ReturnsAssignedValue()
    {
        var attractions = new List<string>
        {
            new(Guid.NewGuid().ToString()),
            new(Guid.NewGuid().ToString())
        };

        var response = new GetEventResponse(
            Guid.NewGuid().ToString(),
            "Halloween Party",
            "2025-12-01",
            "200",
            "1500",
            attractions);

        response.Attractions.Should().BeEquivalentTo(attractions);
    }
    #endregion
}
