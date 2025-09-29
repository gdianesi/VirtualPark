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
    #region GetAll
    [TestMethod]
    public void GetAll_WhenPredicateIsNull_ShouldReturnAll()
    {
        var data = new List<Incidence>
        {
            new() { Id = Guid.NewGuid(), Description = "Incidence 1" },
            new() { Id = Guid.NewGuid(), Description = "Incidence 2" }
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.Is<Expression<Func<Incidence, bool>>?>(p => p == null)))
            .Returns(data);

        var result = _incidenceService.GetAll();

        result.Should().HaveCount(2);
        result.Select(i => i.Description)
            .Should().BeEquivalentTo("Incidence 1", "Incidence 2");

        _mockIncidenceRepository.Verify(
            r => r.GetAll(It.Is<Expression<Func<Incidence, bool>>?>(p => p == null)),
            Times.Once);

        _mockIncidenceRepository.Verify(
            r => r.GetAll(It.Is<Expression<Func<Incidence, bool>>?>(p => p != null)),
            Times.Never);
    }

    [TestMethod]
    public void GetAll_WhenPredicateProvided_ShouldReturnFiltered()
    {
        var data = new List<Incidence>
        {
            new() { Id = Guid.NewGuid(), Description = "Active",   Active = true  },
            new() { Id = Guid.NewGuid(), Description = "Inactive", Active = false }
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns<Expression<Func<Incidence, bool>>>(pred => data.Where(pred.Compile()).ToList());

        Expression<Func<Incidence, bool>> predicate = i => i.Active;

        var result = _incidenceService.GetAll(predicate);

        result.Should().HaveCount(1);
        result[0].Description.Should().Be("Active");

        _mockIncidenceRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()),
            Times.Once);
    }

    #endregion
    #region Get

    [TestMethod]
    public void Get_WhenEntityExists_ShouldReturnEntity()
    {
        var expected = new Incidence { Id = Guid.NewGuid(), Description = "Test" };
        Expression<Func<Incidence, bool>> predicate = i => i.Description == "Test";

        _mockIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns(expected);

        var result = _incidenceService.Get(predicate);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);
        _mockIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void Get_ShouldForwardSamePredicateToRepository()
    {
        var expected = new Incidence { Id = Guid.NewGuid(), Description = "X" };
        Expression<Func<Incidence, bool>> predicate = i => i.Description.StartsWith("X");

        _mockIncidenceRepository
            .Setup(r => r.Get(It.Is<Expression<Func<Incidence, bool>>>(p => p == predicate)))
            .Returns(expected);

        var result = _incidenceService.Get(predicate);

        result.Should().BeSameAs(expected);
        _mockIncidenceRepository.Verify(
            r => r.Get(It.Is<Expression<Func<Incidence, bool>>>(p => p == predicate)), Times.Once);
    }

    [TestMethod]
    public void Get_WhenRepositoryReturnsNull_ShouldReturnNull()
    {
        Expression<Func<Incidence, bool>> predicate = i => i.Id == Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns((Incidence?)null);

        var result = _incidenceService.Get(predicate);

        result.Should().BeNull();
        _mockIncidenceRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
    }

    #endregion
    #region Exist

    [TestMethod]
    public void Exist_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        Expression<Func<Incidence, bool>> predicate = i => i.Active;
        _mockIncidenceRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns(true);

        var result = _incidenceService.Exist(predicate);

        result.Should().BeTrue();
        _mockIncidenceRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
    }

    [TestMethod]
    public void Exist_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        Expression<Func<Incidence, bool>> predicate = i => i.Active;
        _mockIncidenceRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns(false);

        var result = _incidenceService.Exist(predicate);

        result.Should().BeFalse();
        _mockIncidenceRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
    }

    #endregion
    #region Update

        [TestMethod]
        public void Update_WhenEntityExists_ShouldApplyArgsAndCallRepositoryUpdate()
        {
            var id = Guid.NewGuid();
            var existing = new Incidence
            {
                Id = id,
                Description = "Old",
                Start = new DateTime(2020,1,1,8,0,0),
                End = new DateTime(2020,1,1,9,0,0),
                AttractionId = Guid.NewGuid(),
                Active = false,
                Type = null
            };

            _mockIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
                .Returns(existing);

            _mockTypeIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
                .Returns(new TypeIncidence { Id = Guid.Parse(_incidenceArgs.TypeIncidence!.ToString()), Type = "Locked" });

            Incidence? captured = null;
            _mockIncidenceRepository
                .Setup(r => r.Update(It.IsAny<Incidence>()))
                .Callback<Incidence>(i => captured = i);

            _incidenceService.Update(id, _incidenceArgs);

            captured.Should().NotBeNull();
            captured!.Id.Should().Be(id);
            captured.Description.Should().Be(_incidenceArgs.Description);
            captured.Start.Should().Be(_incidenceArgs.Start);
            captured.End.Should().Be(_incidenceArgs.End);
            captured.AttractionId.Should().Be(_incidenceArgs.AttractionId);
            captured.Active.Should().BeTrue();

            _mockIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Update(It.IsAny<Incidence>()), Times.Once);
        }

        [TestMethod]
        public void Update_WhenEntityDoesNotExist_ShouldThrowInvalidOperationException_AndNotCallUpdate()
        {
            var id = Guid.NewGuid();

            _mockIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
                .Returns((Incidence?)null);

            Action act = () => _incidenceService.Update(id, _incidenceArgs);

            act.Should().Throw<InvalidOperationException>()
               .WithMessage($"Incidence with id {id} not found.");

            _mockIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Update(It.IsAny<Incidence>()), Times.Never);
        }

        #endregion
        #region Delete
        [TestMethod]
        public void Remove_WhenEntityExists_ShouldCallRepositoryRemoveOnce()
        {
            var id = Guid.NewGuid();
            var existing = new Incidence { Id = id, Description = "Any" };

            _mockIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
                .Returns(existing);

            Incidence? captured = null;
            _mockIncidenceRepository
                .Setup(r => r.Remove(It.IsAny<Incidence>()))
                .Callback<Incidence>(i => captured = i);

            _incidenceService.Remove(id);

            captured.Should().NotBeNull();
            captured!.Id.Should().Be(id);

            _mockIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Remove(It.IsAny<Incidence>()), Times.Once);
            _mockIncidenceRepository.VerifyAll();
        }

        [TestMethod]
        public void Remove_WhenEntityDoesNotExist_ShouldThrow_AndNotCallRemove()
        {
            var id = Guid.NewGuid();

            _mockIncidenceRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()))
                .Returns((Incidence?)null);

            Action act = () => _incidenceService.Remove(id);

            act.Should().Throw<InvalidOperationException>()
                .WithMessage($"Incidence with id {id} not found.");

            _mockIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>()), Times.Once);
            _mockIncidenceRepository.Verify(r => r.Remove(It.IsAny<Incidence>()), Times.Never);
            _mockIncidenceRepository.VerifyAll();
        }
        #endregion

}
