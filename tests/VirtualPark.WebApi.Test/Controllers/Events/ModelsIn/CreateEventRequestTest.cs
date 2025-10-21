using FluentAssertions;
using VirtualPark.WebApi.Controllers.Events.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Events.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateEventRequest")]
public class CreateEventRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ShouldReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Name = "Halloween Party" };
        request.Name.Should().Be("Halloween Party");
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Date = "2025-10-31" };
        request.Date.Should().Be("2025-10-31");
    }
    #endregion

    #region Capacity
    [TestMethod]
    [TestCategory("Validation")]
    public void Capacity_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Capacity = "200" };
        request.Capacity.Should().Be("200");
    }
    #endregion

    #region Cost
    [TestMethod]
    [TestCategory("Validation")]
    public void Cost_Getter_ReturnsAssignedValue()
    {
        var request = new CreateEventRequest { Cost = "1500" };
        request.Cost.Should().Be("1500");
    }
    #endregion

    #region AttractionsIds
    [TestMethod]
    [TestCategory("Validation")]
    public void AttractionsIds_Getter_ReturnsAssignedValue()
    {
        var guid1 = Guid.NewGuid().ToString();
        var guid2 = Guid.NewGuid().ToString();

        var request = new CreateEventRequest { AttractionsIds = [guid1, guid2] };

        request.AttractionsIds.Should().Contain([guid1, guid2]);
    }
    #endregion

    #region ToArgs
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldReturnEventsArgs_WithValidatedValues()
    {
        var guid = Guid.NewGuid().ToString();

        var request = new CreateEventRequest
        {
            Name = "Halloween Party",
            Date = "2025-10-31",
            Capacity = "200",
            Cost = "1500",
            AttractionsIds = [guid]
        };

        var result = request.ToArgs();

        result.Should().NotBeNull();
        result.Name.Should().Be("Halloween Party");
        result.Date.Should().Be(new DateOnly(2025, 10, 31));
        result.Capacity.Should().Be(200);
        result.Cost.Should().Be(1500);
        result.AttractionIds.Should().Contain(Guid.Parse(guid));
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenAnyFieldIsEmpty()
    {
        var request = new CreateEventRequest
        {
            Name = string.Empty,
            Date = string.Empty,
            Capacity = string.Empty,
            Cost = string.Empty,
            AttractionsIds = []
        };

        Action act = () => request.ToArgs();

        act.Should().Throw<ArgumentException>();
    }
    #endregion

}
