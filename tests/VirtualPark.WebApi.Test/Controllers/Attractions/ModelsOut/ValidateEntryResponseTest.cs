using FluentAssertions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions.ModelsOut;

[TestClass]
[TestCategory("ValidateEntryResponse")]
public class ValidateEntryResponseTest
{
    #region IsValid
    [TestMethod]
    public void ValidateEntryResponse_IsValidProperty_GetAndSet_ShouldWorkCorrectly()
    {
        var response = new ValidateEntryResponse { IsValid = true };

        response.IsValid.Should().BeTrue();
    }
    #endregion
}
