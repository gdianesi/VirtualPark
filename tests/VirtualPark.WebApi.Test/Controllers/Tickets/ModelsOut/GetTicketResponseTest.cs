using FluentAssertions;
using VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetTicketResponse")]
public sealed class GetTicketResponseTest
{
    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();
        var response = new GetTicketResponse(
            id);

        response.Id.Should().Be(id);
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        const string type = "General";
        var response = new GetTicketResponse(
            Guid.NewGuid().ToString(), type);

        response.Type.Should().Be(type);
    }
    #endregion
}
