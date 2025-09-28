using FluentAssertions;
using VirtualPark.BusinessLogic.TypeIncidences.Models;

namespace VirtualPark.BusinessLogic.Test.TypeIncidences.Models;

[TestClass]
[TestCategory("TypeIncidenceArgsTest")]
public class TypeIncidenceArgsTest
{
    #region Type
    [TestMethod]
    public void Constructor_ShouldSetTypeProperty()
    {
        var typeIncidenceArgs = new TypeIncidenceArgs("Locked");

        typeIncidenceArgs.Type.Should().Be("Locked");
    }
    #endregion
}
