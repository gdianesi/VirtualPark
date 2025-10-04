using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
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
    private Mock<IReadOnlyRepository<Attraction>> _mockAttractionRepository = null!;
    private IncidenceService _incidenceService = null!;
    private IncidenceArgs _incidenceArgs = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockIncidenceRepository = new Mock<IRepository<Incidence>>(MockBehavior.Strict);
        _mockTypeIncidenceRepository = new Mock<IReadOnlyRepository<TypeIncidence>>(MockBehavior.Strict);
        _mockAttractionRepository = new Mock<IReadOnlyRepository<Attraction>>(MockBehavior.Strict);

        _incidenceService = new IncidenceService(
            _mockIncidenceRepository.Object,
            _mockTypeIncidenceRepository.Object,
            _mockAttractionRepository.Object);

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
    [TestCategory("Validation")]
    public void Create_WhenTypeIncidenceExists_ShouldAddAndReturnNewId_WithTypeSet()
    {
        var typeId = Guid.NewGuid();
        var attractionId = Guid.NewGuid();

        var expectedType = new TypeIncidence { Id = typeId, Type = "Locked" };
        var attraction = new Attraction { Id = attractionId, Name = "Attraction Name" };

        var start = DateTime.Now;
        var end = start.AddHours(1);

        var args = new IncidenceArgs(
            typeIncidence: typeId.ToString(),
            description: "Puerta trabada",
            start: start.ToString("yyyy-MM-dd HH:mm:ss"),
            end: end.ToString("yyyy-MM-dd HH:mm:ss"),
            attractionId: attractionId.ToString(),
            active: "true");

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(expectedType);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockIncidenceRepository
            .Setup(r => r.Add(It.Is<Incidence>(i =>
                i.Description == args.Description &&
                i.Start == args.Start &&
                i.End == args.End &&
                i.Active == args.Active &&
                i.AttractionId == args.AttractionId &&
                i.Attraction == attraction &&
                i.Type != null &&
                i.Type!.Id == typeId &&
                i.TypeIncidenceId == args.TypeIncidence)));

        var result = _incidenceService.Create(args);

        result.Should().NotBeEmpty();

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockAttractionRepository.Verify(r => r.Get(a => a.Id == attractionId), Times.Once);
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Create_WhenTypeIncidenceNotFound_ShouldAddWithNullType_AndReturnNewId()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var attractionId = _incidenceArgs.AttractionId;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns((TypeIncidence?)null);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(new Attraction { Id = attractionId, Name = "Any" });

        Incidence? added = null;
        _mockIncidenceRepository
            .Setup(r => r.Add(It.Is<Incidence>(i => true)))
            .Callback<Incidence>(i => added = i);

        var id = _incidenceService.Create(_incidenceArgs);

        id.Should().NotBe(Guid.Empty);
        added.Should().NotBeNull();
        added!.Type.Should().BeNull();
        added.Description.Should().Be(_incidenceArgs.Description);

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockAttractionRepository.Verify(r => r.Get(a => a.Id == attractionId), Times.Once);
        _mockIncidenceRepository.Verify(r => r.Add(It.Is<Incidence>(i => true)), Times.Once);
    }
    #endregion

    #region FindTypeIncidenceById
    [TestMethod]
    public void FindTypeIncidenceById_WhenEntityExists_ShouldReturnEntity()
    {
        var id = Guid.NewGuid();
        var expected = new TypeIncidence { Id = id, Type = "Locked" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns(expected);

        var result = _incidenceService.FindTypeIncidenceById(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Type.Should().Be("Locked");

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == id), Times.Once);
    }
    #endregion

    #region MapToEntity
    [TestMethod]
    public void MapToEntity_WhenTypeIncidenceExists_ShouldMapAllFields_AndSetType()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var attractionId = _incidenceArgs.AttractionId;

        var expectedType = new TypeIncidence { Id = typeId, Type = "Locked" };
        var expectedAttraction = new Attraction { Id = attractionId, Name = "X" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(expectedType);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(expectedAttraction);

        var entity = _incidenceService.MapToEntity(_incidenceArgs);

        entity.Should().NotBeNull();
        entity.Description.Should().Be(_incidenceArgs.Description);
        entity.Start.Should().Be(_incidenceArgs.Start);
        entity.End.Should().Be(_incidenceArgs.End);
        entity.Active.Should().Be(_incidenceArgs.Active);
        entity.AttractionId.Should().Be(attractionId);
        entity.Attraction.Should().Be(expectedAttraction);
        entity.Type.Should().Be(expectedType);

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockAttractionRepository.Verify(r => r.Get(a => a.Id == attractionId), Times.Once);
    }

    [TestMethod]
    public void MapToEntity_WhenTypeIncidenceNotFound_ShouldMapAllFields_AndLeaveTypeNull()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var attractionId = _incidenceArgs.AttractionId;

        var expectedAttraction = new Attraction { Id = attractionId, Name = "X" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns((TypeIncidence?)null);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(expectedAttraction);

        var entity = _incidenceService.MapToEntity(_incidenceArgs);

        entity.Should().NotBeNull();
        entity.Description.Should().Be(_incidenceArgs.Description);
        entity.Start.Should().Be(_incidenceArgs.Start);
        entity.End.Should().Be(_incidenceArgs.End);
        entity.AttractionId.Should().Be(attractionId);
        entity.Attraction.Should().Be(expectedAttraction);
        entity.Type.Should().BeNull();

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockAttractionRepository.Verify(r => r.Get(a => a.Id == attractionId), Times.Once);
    }
    #endregion

    #region ApplyArgsToEntity
    [TestMethod]
    public void ApplyArgsToEntity_WhenTypeIncidenceExists_ShouldSetAllFields_AndType()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var existingType = new TypeIncidence { Id = typeId, Type = "Locked" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(existingType);

        var entity = new Incidence
        {
            Description = "Old",
            Start = new DateTime(2020, 1, 1, 10, 0, 0),
            End = new DateTime(2020, 1, 1, 12, 0, 0),
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

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void ApplyArgsToEntity_WhenTypeIncidenceNotFound_ShouldSetAllFields_AndLeaveTypeNull()
    {
        var typeId = _incidenceArgs.TypeIncidence;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns((TypeIncidence?)null);

        var entity = new Incidence
        {
            Description = "Old",
            Start = new DateTime(2020, 1, 1, 10, 0, 0),
            End = new DateTime(2020, 1, 1, 12, 0, 0),
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

        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_ShouldReturnAll()
    {
        var data = new List<Incidence>
        {
            new() { Id = Guid.NewGuid(), Description = "Incidence 1" },
            new() { Id = Guid.NewGuid(), Description = "Incidence 2" }
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(null))
            .Returns(data);

        var result = _incidenceService.GetAll();

        result.Should().HaveCount(2);
        result.Select(i => i.Description).Should().BeEquivalentTo("Incidence 1", "Incidence 2");

        _mockIncidenceRepository.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void Get_WhenEntityExists_ShouldReturnEntity()
    {
        var id = Guid.NewGuid();
        var expected = new Incidence { Id = id, Description = "Test" };

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns(expected);

        var result = _incidenceService.Get(id);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
    }

    [TestMethod]
    public void Get_WhenEntityDoesNotExist_ShouldThrow()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns((Incidence?)null);

        Action act = () => _incidenceService.Get(id);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
    }
    #endregion

    #region Exist
    [TestMethod]
    public void Exist_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Exist(i => i.Id == id))
            .Returns(true);

        var result = _incidenceService.Exist(id);

        result.Should().BeTrue();

        _mockIncidenceRepository.Verify(r => r.Exist(i => i.Id == id), Times.Once);
    }

    [TestMethod]
    public void Exist_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Exist(i => i.Id == id))
            .Returns(false);

        var result = _incidenceService.Exist(id);

        result.Should().BeFalse();

        _mockIncidenceRepository.Verify(r => r.Exist(i => i.Id == id), Times.Once);
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenEntityExists_ShouldApplyArgsAndCallRepositoryUpdate()
    {
        var id = Guid.NewGuid();
        var existing = new Incidence { Id = id };

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns(existing);

        var typeId = _incidenceArgs.TypeIncidence;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(new TypeIncidence { Id = typeId, Type = "Locked" });

        Incidence? captured = null;
        _mockIncidenceRepository
            .Setup(r => r.Update(It.Is<Incidence>(x => x.Id == id)))
            .Callback<Incidence>(i => captured = i);

        _incidenceService.Update(id, _incidenceArgs);

        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);
        captured.Description.Should().Be(_incidenceArgs.Description);
        captured.Start.Should().Be(_incidenceArgs.Start);
        captured.End.Should().Be(_incidenceArgs.End);
        captured.AttractionId.Should().Be(_incidenceArgs.AttractionId);
        captured.Active.Should().BeTrue();

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
        _mockTypeIncidenceRepository.Verify(r => r.Get(t => t.Id == typeId), Times.Once);
        _mockIncidenceRepository.Verify(r => r.Update(It.Is<Incidence>(x => x.Id == id)), Times.Once);
    }

    [TestMethod]
    public void Update_WhenEntityDoesNotExist_ShouldThrowInvalidOperationException_AndNotCallUpdate()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns((Incidence?)null);

        Action act = () => _incidenceService.Update(id, _incidenceArgs);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
        _mockIncidenceRepository.Verify(r => r.Update(It.Is<Incidence>(x => x.Id == id)), Times.Never);
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Remove_WhenEntityExists_ShouldCallRepositoryRemoveOnce()
    {
        var id = Guid.NewGuid();
        var existing = new Incidence { Id = id };

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns(existing);

        Incidence? captured = null;
        _mockIncidenceRepository
            .Setup(r => r.Remove(It.Is<Incidence>(x => x.Id == id)))
            .Callback<Incidence>(i => captured = i);

        _incidenceService.Remove(id);

        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
        _mockIncidenceRepository.Verify(r => r.Remove(It.Is<Incidence>(x => x.Id == id)), Times.Once);
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Remove_WhenEntityDoesNotExist_ShouldThrow_AndNotCallRemove()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(i => i.Id == id))
            .Returns((Incidence?)null);

        Action act = () => _incidenceService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.Verify(r => r.Get(i => i.Id == id), Times.Once);
        _mockIncidenceRepository.Verify(r => r.Remove(It.Is<Incidence>(x => x.Id == id)), Times.Never);
        _mockIncidenceRepository.VerifyAll();
    }
    #endregion
}
