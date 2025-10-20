namespace VirtualPark.WebApi.Test.Controllers.Reward.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateRewardRequest")]
public class CreateRewardRequestTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Name_Getter_ReturnsAssignedValue()
    {
        var request = new CreateRewardRequest { Name = "VIP Ticket" };
        request.Name.Should().Be("VIP Ticket");
    }
    #endregion
}
