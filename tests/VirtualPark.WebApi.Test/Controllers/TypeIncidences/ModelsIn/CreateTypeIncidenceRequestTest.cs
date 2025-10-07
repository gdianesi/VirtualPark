using FluentAssertions;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsIn;

namespace VirtualPark.WebApi.Test.Controllers.TypeIncidences.ModelsIn;

[TestClass]
[TestCategory("ModelsIn")]
[TestCategory("CreateTypeIncidenceRequest")]
public class CreateTypeIncidenceRequestTest
{
    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var createTypeIncidenceRequest = new CreateTypeIncidenceRequest() { Type = "type" };
        createTypeIncidenceRequest.Type.Should().Be("type");
    }
    #endregion

    #region ToArgs
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldMapType()
    {
        var request = new CreateTypeIncidenceRequest
        {
            Type = "type"
        };

        var args = request.ToArgs();

        args.Should().NotBeNull();
        args.Should().BeOfType<TypeIncidenceArgs>();
        args.Type.Should().Be("type");
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenTypeIsNull()
    {
        var request = new CreateTypeIncidenceRequest
        {
            Type = null
        };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void ToArgs_ShouldThrow_WhenTypeIsEmpty()
    {
        var request = new CreateTypeIncidenceRequest
        {
            Type = string.Empty
        };

        var act = request.ToArgs;

        act.Should().Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion
}
