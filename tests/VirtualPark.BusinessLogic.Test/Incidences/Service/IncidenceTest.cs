using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.Repository;
using Incidence = VirtualPark.BusinessLogic.Incidences.Entity.Incidence;

namespace VirtualPark.BusinessLogic.Test.Incidences.Service;

[TestClass]
[TestCategory("Incidence")]
public sealed class IncidenceTest
{
    private Mock<IRepository<Incidence>> _mockIncidenceRepository = null!;
    private Mock<IReadOnlyRepository<TypeIncidence>> _mockTypeIncidenceRepository = null!;
    private IncidenceService _incidenceService = null!;
    private IncidenceArgs _incidenceArgs = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockIncidenceRepository = new Mock<IRepository<Incidence>>(MockBehavior.Strict);
        _mockTypeIncidenceRepository = new Mock<IReadOnlyRepository<TypeIncidence>>(MockBehavior.Strict);

        _incidenceService = new IncidenceService(
            _mockIncidenceRepository.Object,
            _mockTypeIncidenceRepository.Object
        );

        _incidenceArgs = new IncidenceArgs(
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "Description",
            "2025-09-27 15:30",
            "2025-09-27 15:30",
            "c8a0b0ef-9a4d-46e0-b9d3-0dfd68b6a010",
            "true");
    }

    #region Create

    [TestMethod]
    public void Create_WhenNotExists_ShouldMapArgsAndAddOnce()
    {
        Incidence? captured = null;

        _mockIncidenceRepository
            .Setup(r => r.Add(It.IsAny<Incidence>()))
            .Callback<Incidence>(i => captured = i);

        var incidence = _incidenceService.Create(_incidenceArgs);

        incidence.Should().NotBeNull();
        incidence.Description.Should().Be(_incidenceArgs.Description);
        incidence.Start.Should().Be(_incidenceArgs.Start);
        incidence.End.Should().Be(_incidenceArgs.End);
        incidence.Active.Should().BeTrue();
        incidence.AttractionId.Should().Be(_incidenceArgs.AttractionId);

        _mockIncidenceRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
        _mockIncidenceRepository.Verify(
            r => r.Add(It.Is<Incidence>(i =>
                i.Description == _incidenceArgs.Description &&
                i.Start == _incidenceArgs.Start &&
                i.End == _incidenceArgs.End &&
                i.Active == _incidenceArgs.Active &&
                i.AttractionId == _incidenceArgs.AttractionId
            )),
            Times.Once);

        _mockIncidenceRepository.VerifyAll();
    }

    #endregion
    #region FindTypeIncidenceById
    [TestMethod]
    public void FindTypeIncidenceById_WhenEntityExists_ShouldReturnEntity()
    {
        var id = Guid.NewGuid();
        var expected = new TypeIncidence { Id = id, Type = "Locked" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns(expected);

        var result = _typeIncidenceService.FindTypeIncidenceById(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Type.Should().Be("Locked");

        _mockTypeIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
            Times.Once);
    }
    #endregion
}
