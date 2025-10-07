using FluentAssertions;
using VirtualPark.WebApi.Controllers.Tickets.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Tickets.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("CreateTicketResponse")]
public sealed class CreateTicketResponseTest
{
    #region Id

    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid().ToString();

        var response = new CreateTicketResponse(id);

        response.Id.Should().Be(id);
    }

    #endregion
}
