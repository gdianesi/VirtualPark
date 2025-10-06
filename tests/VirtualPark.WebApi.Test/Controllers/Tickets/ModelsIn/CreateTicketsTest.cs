using FluentAssertions;
using VirtualPark.WebApi.Controllers.Tickets.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateTicketRequest")]
public class CreateTicketRequestTest
{
    #region VisitorId
    [TestMethod]
    [TestCategory("Validation")]
    public void VisitorId_Getter_ShouldReturnAssignedValue()
    {
        var guid = Guid.NewGuid().ToString();
        var request = new CreateTicketRequest(guid);
        request.VisitorId.Should().Be(guid);
    }
    #endregion
}
