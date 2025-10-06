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
            id, "General", "2025-12-01");

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
            Guid.NewGuid().ToString(), type, "2025-12-01");

        response.Type.Should().Be(type);
    }
    #endregion

    #region Date
    [TestMethod]
    [TestCategory("Validation")]
    public void Date_Getter_ReturnsAssignedValue()
    {
        const string date = "2025-12-01";
        var response = new GetTicketResponse(
            Guid.NewGuid().ToString(), "Event", date);

        response.Date.Should().Be(date);
    }
    #endregion

    #region QrId
    [TestMethod]
    [TestCategory("Validation")]
    public void QrId_Getter_ReturnsAssignedValue()
    {
        var qrId = Guid.NewGuid().ToString();
        var response = new GetTicketResponse(
            Guid.NewGuid().ToString(), "Event", "2025-12-01", Guid.NewGuid().ToString(), qrId);

        response.QrId.Should().Be(qrId);
    }
    #endregion
}
