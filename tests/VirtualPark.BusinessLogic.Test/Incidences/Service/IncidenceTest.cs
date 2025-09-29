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
        public void Create_WhenTypeIncidenceExists_ShouldAddAndReturnNewId_WithTypeSet()
        {
            Guid _typeId = Guid.NewGuid();
            var expectedType = new TypeIncidence { Id = _typeId, Type = "Locked" };

            _mockTypeIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
                .Returns(expectedType);

            Incidence? added = null;
            _mockIncidenceRepository
                .Setup(r => r.Add(It.IsAny<Incidence>()))
                .Callback<Incidence>(i => added = i);

            Guid id = _incidenceService.Create(_incidenceArgs);

            id.Should().NotBe(Guid.Empty);
            added.Should().NotBeNull();
            added!.Id.Should().Be(id);
            added.Description.Should().Be(_incidenceArgs.Description);
            added.Start.Should().Be(_incidenceArgs.Start);
            added.End.Should().Be(_incidenceArgs.End);
            added.Active.Should().BeTrue();
            added.AttractionId.Should().Be(_incidenceArgs.AttractionId);
            added.Type.Should().NotBeNull();
            added.Type!.Id.Should().Be(_typeId);

            _mockTypeIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Add(It.IsAny<Incidence>()), Times.Once);
            _mockTypeIncidenceRepository.VerifyAll();
            _mockIncidenceRepository.VerifyAll();
        }

        [TestMethod]
        public void Create_WhenTypeIncidenceNotFound_ShouldAddWithNullType_AndReturnNewId()
        {
            _mockTypeIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
                .Returns((TypeIncidence?)null);

            Incidence? added = null;
            _mockIncidenceRepository
                .Setup(r => r.Add(It.IsAny<Incidence>()))
                .Callback<Incidence>(i => added = i);

            Guid id = _incidenceService.Create(_incidenceArgs);

            id.Should().NotBe(Guid.Empty);
            added.Should().NotBeNull();
            added!.Id.Should().Be(id);
            added.Type.Should().BeNull();
            added.Description.Should().Be(_incidenceArgs.Description);

            _mockTypeIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Add(It.IsAny<Incidence>()), Times.Once);
            _mockTypeIncidenceRepository.VerifyAll();
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

        var result = _incidenceService.FindTypeIncidenceById(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Type.Should().Be("Locked");

        _mockTypeIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
            Times.Once);
    }
    #endregion
    #region MapToEntity

    [TestMethod]
    public void MapToEntity_WhenTypeIncidenceExists_ShouldMapAllFields_AndSetType()
    {
        var typeId = Guid.NewGuid();
        var expectedType = new TypeIncidence { Id = typeId, Type = "Locked" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns(expectedType);

        var entity = _incidenceService.MapToEntity(_incidenceArgs);

        entity.Should().NotBeNull();
        entity.Description.Should().Be(_incidenceArgs.Description);
        entity.Start.Should().Be(_incidenceArgs.Start);
        entity.End.Should().Be(_incidenceArgs.End);
        entity.Active.Should().BeTrue();
        entity.AttractionId.Should().Be(_incidenceArgs.AttractionId);

        entity.Type.Should().NotBeNull();
        entity.Type!.Id.Should().Be(typeId);

        _mockTypeIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
            Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void MapToEntity_WhenTypeIncidenceNotFound_ShouldMapAllFields_AndLeaveTypeNull()
    {
        _mockTypeIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns((TypeIncidence?)null);

        var entity = _incidenceService.MapToEntity(_incidenceArgs);

        entity.Should().NotBeNull();
        entity.Description.Should().Be(_incidenceArgs.Description);
        entity.Start.Should().Be(_incidenceArgs.Start);
        entity.End.Should().Be(_incidenceArgs.End);
        entity.Active.Should().BeTrue();
        entity.AttractionId.Should().Be(_incidenceArgs.AttractionId);

        entity.Type.Should().BeNull();

        _mockTypeIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
            Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    #endregion
    #region ApplyArgsToEntity

[TestMethod]
public void ApplyArgsToEntity_WhenTypeIncidenceExists_ShouldSetAllFields_AndType()
{
    var typeId = Guid.NewGuid();
    var existingType = new TypeIncidence { Id = typeId, Type = "Locked" };

    _mockTypeIncidenceRepository
        .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
        .Returns(existingType);

    var entity = new Incidence
    {
        // Valores distintos para comprobar que se sobreescriben
        Description = "Old",
        Start = new DateTime(2020, 1, 1, 10, 0, 0),
        End   = new DateTime(2020, 1, 1, 12, 0, 0),
        AttractionId = Guid.NewGuid(),
        Active = false,
        Type = null
    };

    _incidenceService.ApplyArgsToEntity(entity, _incidenceArgs);

    entity.Description.Should().Be(_incidenceArgs.Description);
    entity.Start.Should().Be(_incidenceArgs.Start);
    entity.End.Should().Be(_incidenceArgs.End);
    entity.AttractionId.Should().Be(_incidenceArgs.AttractionId);
    entity.Active.Should().BeTrue();

    entity.Type.Should().NotBeNull();
    entity.Type!.Id.Should().Be(existingType.Id);

    _mockTypeIncidenceRepository.Verify(
        r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
        Times.Once);
    _mockTypeIncidenceRepository.VerifyAll();
}

[TestMethod]
public void ApplyArgsToEntity_WhenTypeIncidenceNotFound_ShouldSetAllFields_AndLeaveTypeNull()
{
    _mockTypeIncidenceRepository
        .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
        .Returns((TypeIncidence?)null);

    var entity = new Incidence
    {
        Description = "Old",
        Start = new DateTime(2020, 1, 1, 10, 0, 0),
        End   = new DateTime(2020, 1, 1, 12, 0, 0),
        AttractionId = Guid.NewGuid(),
        Active = false,
        Type = new TypeIncidence { Id = Guid.NewGuid(), Type = "Something" }
    };

    _incidenceService.ApplyArgsToEntity(entity, _incidenceArgs);

    entity.Description.Should().Be(_incidenceArgs.Description);
    entity.Start.Should().Be(_incidenceArgs.Start);
    entity.End.Should().Be(_incidenceArgs.End);
    entity.AttractionId.Should().Be(_incidenceArgs.AttractionId);
    entity.Active.Should().BeTrue();

    entity.Type.Should().BeNull();

    _mockTypeIncidenceRepository.Verify(
        r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
        Times.Once);
    _mockTypeIncidenceRepository.VerifyAll();
}

#endregion

}
