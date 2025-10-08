using FluentAssertions;
using VirtualPark.WebApi.Controllers.Strategy.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Strategy.ModelsOut;

[TestClass]
[TestCategory("Strategy")]
[TestCategory("CreateActiveStrategyResponse")]
public class CreateActiveStrategyResponseTest
{
    [TestMethod]
    public void Constructor_ShouldAssignId_WhenValueIsValid()
    {
        var id = Guid.NewGuid().ToString();

        var response = new CreateActiveStrategyResponse(id);

        response.Id.Should().Be(id);
    }

    [TestMethod]
    public void Id_ShouldBeMutable_WhenPropertyIsSet()
    {
        var response = new CreateActiveStrategyResponse(Guid.NewGuid().ToString());
        var newId = Guid.NewGuid().ToString();

        response.Id = newId;

        response.Id.Should().Be(newId);
    }

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void Constructor_ShouldAllowEmptyOrNullIds_ButYouMayValidateExternally(string? invalidId)
    {
        var response = new CreateActiveStrategyResponse(invalidId!);

        response.Id.Should().Be(invalidId);
    }
}
