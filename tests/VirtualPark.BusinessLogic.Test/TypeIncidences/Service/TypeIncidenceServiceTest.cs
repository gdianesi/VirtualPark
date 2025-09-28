using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.TypeIncidences.Service;

[TestClass]
[TestCategory("TypeIncidenceService")]
public class TypeIncidenceServiceTest
{
    private Mock<IRepository<TypeIncidence>> _mockTypeIncidenceRepository = null!;
    private TypeIncidenceService _typeIncidenceService = null!;
    private TypeIncidenceArgs _typeIncidenceArgs = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockTypeIncidenceRepository = new Mock<IRepository<TypeIncidence>>(MockBehavior.Strict);
        _typeIncidenceService = new TypeIncidenceService(_mockTypeIncidenceRepository.Object);
        _typeIncidenceArgs = new TypeIncidenceArgs(type: "Locked");
    }

    #region Create

    [TestMethod]
    public void Create_WhenArgsAreValid_ShouldCreateTypeIncidence()
    {
        TypeIncidence? captured = null;

        _mockTypeIncidenceRepository
            .Setup(x => x.Add(It.IsAny<TypeIncidence>()))
            .Callback<TypeIncidence>(ti => captured = ti);

        var typeIncidence = _typeIncidenceService.Create(_typeIncidenceArgs);

        typeIncidence.Should().NotBeNull();
        typeIncidence.Id.Should().NotBe(Guid.Empty); 
        typeIncidence.Type.Should().Be(_typeIncidenceArgs.Type);

        captured.Should().NotBeNull();
        captured!.Type.Should().Be(_typeIncidenceArgs.Type);

        _mockTypeIncidenceRepository.Verify(x => x.Add(It.IsAny<TypeIncidence>()), Times.Once);
    }
    #endregion
    #region MapToEntity

    [TestMethod]
    public void MapToEntity_WhenArgsAreValid_ShouldMapToEntity()
    {
        var typeIncidence = _typeIncidenceService.MapToEntity(_typeIncidenceArgs);
        typeIncidence.Should().NotBeNull();
        typeIncidence.Id.Should().NotBe(Guid.Empty);
        typeIncidence.Type.Should().Be(_typeIncidenceArgs.Type);
    }
    #endregion

}
