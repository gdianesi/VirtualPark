using FluentAssertions;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.TypeIncidences.ModelsOut;

[TestClass]
[TestCategory("ModelsOut")]
[TestCategory("GetTypeIncidenceResponse")]
public class GetTypeIncidenceResponseTest
{
    private static TypeIncidence BuildTypeIncidence(Guid? id = null, string? type = null)
    {
        return new TypeIncidence
        {
            Id = id ?? Guid.NewGuid(),
            Type = type ?? "DefaultType"
        };
    }

    #region Id
    [TestMethod]
    [TestCategory("Validation")]
    public void Id_Getter_ReturnsAssignedValue()
    {
        var id = Guid.NewGuid();
        var typeIncidence = BuildTypeIncidence(id: id);

        var response = new GetTypeIncidenceResponse(typeIncidence);

        response.Id.Should().Be(id.ToString());
    }
    #endregion

    #region Type
    [TestMethod]
    [TestCategory("Validation")]
    public void Type_Getter_ReturnsAssignedValue()
    {
        var type = "MechanicalFailure";
        var typeIncidence = BuildTypeIncidence(type: type);

        var response = new GetTypeIncidenceResponse(typeIncidence);

        response.Type.Should().Be(type);
    }
    #endregion
}
