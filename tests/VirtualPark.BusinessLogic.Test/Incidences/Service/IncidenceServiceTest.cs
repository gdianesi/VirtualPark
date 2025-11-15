using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
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

        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenTypeIncidenceNotFound_ShouldThrow_AndNotAdd()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var attractionId = _incidenceArgs.AttractionId;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns((TypeIncidence?)null);

        Action act = () => _incidenceService.Create(_incidenceArgs);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Type incidence don't exist");

        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_WhenAttractionDoesNotExist_ShouldThrow()
    {
        var typeId = _incidenceArgs.TypeIncidence;
        var attractionId = _incidenceArgs.AttractionId;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(new TypeIncidence { Id = typeId, Type = "Locked" });

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns((Attraction?)null);

        Action act = () => _incidenceService.Create(_incidenceArgs);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _mockIncidenceRepository.VerifyAll();
        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
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

        _mockTypeIncidenceRepository.Verify();
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

        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
    }

    [TestMethod]
    public void MapToEntity_WhenTypeIncidenceNotFound_ShouldThrow()
    {
        var typeId = _incidenceArgs.TypeIncidence;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns((TypeIncidence?)null);

        Action act = () => _incidenceService.MapToEntity(_incidenceArgs);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Type incidence don't exist");

        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockIncidenceRepository.VerifyAll();
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

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void ApplyArgsToEntity_WhenTypeIncidenceNotFound_ShouldThrow()
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

        Action act = () => _incidenceService.ApplyArgsToEntity(entity, _incidenceArgs);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Type incidence don't exist");

        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockIncidenceRepository.VerifyAll();
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
            .Setup(r => r.GetAll())
            .Returns(data);

        _mockIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Incidence, bool>>>(), It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns((Expression<Func<Incidence, bool>> predicate, Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>? include) => data.AsQueryable().FirstOrDefault(predicate.Compile()));

        var result = _incidenceService.GetAll();

        result.Should().HaveCount(2);
        result.Select(i => i.Description).Should().BeEquivalentTo("Incidence 1", "Incidence 2");

        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("AutoDeactivate")]
    public void GetAll_WhenIncidenceExpired_ShouldSetActiveFalse_AndCallUpdate()
    {
        var id = Guid.NewGuid();
        var expired = new Incidence
        {
            Id = id,
            Start = DateTime.Now.AddHours(-2),
            End = DateTime.Now.AddHours(-1),
            Active = true
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll())
            .Returns([expired]);

        _mockIncidenceRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Incidence, bool>>>(),
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns(expired);

        _mockIncidenceRepository
            .Setup(r => r.Update(It.Is<Incidence>(x => x.Id == id && x.Active == false)));

        var result = _incidenceService.GetAll();

        result.Should().HaveCount(1);
        result.First().Active.Should().BeFalse();

        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("AutoDeactivate")]
    public void GetAll_WhenIncidenceNotExpired_ShouldRemainActive_AndNotCallUpdate()
    {
        var id = Guid.NewGuid();
        var active = new Incidence
        {
            Id = id,
            Start = DateTime.Now.AddHours(-1),
            End = DateTime.Now.AddHours(1),
            Active = true
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll())
            .Returns([active]);

        _mockIncidenceRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Incidence, bool>>>(),
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns(active);

        _mockIncidenceRepository
            .Setup(r => r.Update(It.IsAny<Incidence>()))
            .Verifiable("Update should NOT be called");

        var result = _incidenceService.GetAll();

        result.First().Active.Should().BeTrue();
    }

    #endregion

    #region Get
    [TestMethod]
    public void Get_WhenEntityExists_ShouldReturnEntity()
    {
        var id = Guid.NewGuid();
        var expected = new Incidence { Id = id, Description = "Test" };

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Incidence, object>>>()))
            .Returns(expected);

        var result = _incidenceService.Get(id);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);

        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("AutoDeactivate")]
    public void Get_WhenExpired_ShouldDeactivateAndUpdate()
    {
        var id = Guid.NewGuid();

        var expired = new Incidence
        {
            Id = id,
            Active = true,
            Start = DateTime.Now.AddHours(-2),
            End = DateTime.Now.AddHours(-1)
        };

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns(expired);

        _mockIncidenceRepository
            .Setup(r => r.Update(It.Is<Incidence>(x => x.Id == id && x.Active == false)));

        var result = _incidenceService.Get(id);

        result.Active.Should().BeFalse();
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_WhenEntityDoesNotExist_ShouldThrow()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Incidence, object>>>()))
            .Returns((Incidence?)null);

        Action act = () => _incidenceService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.VerifyAll();
        _mockTypeIncidenceRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenEntityExists_ShouldApplyArgsAndCallRepositoryUpdate()
    {
        var id = Guid.NewGuid();
        var existing = new Incidence { Id = id };

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns(existing);

        var typeId = _incidenceArgs.TypeIncidence;
        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == typeId))
            .Returns(new TypeIncidence { Id = typeId, Type = "Locked" });

        _mockIncidenceRepository
            .Setup(r => r.Update(It.Is<Incidence>(x => x.Id == id)));

        _incidenceService.Update(_incidenceArgs, id);

        _mockTypeIncidenceRepository.VerifyAll();
        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_WhenEntityDoesNotExist_ShouldThrowInvalidOperationException_AndNotCallUpdate()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns((Incidence?)null);

        var act = () => _incidenceService.Update(_incidenceArgs, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.VerifyAll();
    }
    #endregion

    #region Delete
    [TestMethod]
    public void Remove_WhenEntityExists_ShouldCallRepositoryRemoveOnce()
    {
        var id = Guid.NewGuid();
        var existing = new Incidence { Id = id };

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns(existing);

        _mockIncidenceRepository
            .Setup(r => r.Remove(existing));

        _incidenceService.Remove(id);

        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Remove_WhenEntityDoesNotExist_ShouldThrow_AndNotCallRemove()
    {
        var id = Guid.NewGuid();

        _mockIncidenceRepository
            .Setup(r => r.Get(
                i => i.Id == id,
                It.IsAny<Func<IQueryable<Incidence>, IIncludableQueryable<Incidence, object>>>()))
            .Returns((Incidence?)null);

        Action act = () => _incidenceService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Incidence don't exist");

        _mockIncidenceRepository.VerifyAll();
    }
    #endregion

    #region HasActiveIncidenceForAttraction
    #region Success
    [TestMethod]
    [TestCategory("Maintenance")]
    public void HasActiveIncidenceForAttraction_WhenActiveIncidenceOverlapsDate_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var now = new DateTime(2025, 01, 01, 10, 00, 00);

        var incidencesInDb = new List<Incidence>
        {
            new()
            {
                Id = Guid.NewGuid(),
                AttractionId = attractionId,
                Start = now.AddHours(-1),
                End = now.AddHours(1),
                Active = true,
                Description = "Mantenimiento preventivo",
                TypeIncidenceId = Guid.NewGuid(),
            }
        };
        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns((Expression<Func<Incidence, bool>>? filter) =>
            {
                var query = incidencesInDb.AsQueryable();
                return filter == null ? query.ToList() : query.Where(filter).ToList();
            });

        var result = _incidenceService.HasActiveIncidenceForAttraction(attractionId, now);

        result.Should().BeTrue();
        _mockAttractionRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("AutoDeactivate")]
    public void HasActiveIncidenceForAttraction_WhenExpired_ShouldDeactivateAndReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var now = DateTime.Now;

        var expired = new Incidence
        {
            Id = Guid.NewGuid(),
            AttractionId = attractionId,
            Active = true,
            Start = now.AddHours(-3),
            End = now.AddHours(-1)
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns([expired]);

        _mockIncidenceRepository
            .Setup(r => r.Update(It.Is<Incidence>(x => x.Active == false)));

        var result = _incidenceService.HasActiveIncidenceForAttraction(attractionId, now);

        result.Should().BeFalse();
        expired.Active.Should().BeFalse();

        _mockIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("AutoDeactivate")]
    public void HasActiveIncidenceForAttraction_WhenActiveAndValid_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var now = DateTime.Now;

        var valid = new Incidence
        {
            Id = Guid.NewGuid(),
            AttractionId = attractionId,
            Active = true,
            Start = now.AddHours(-1),
            End = now.AddHours(1)
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns([valid]);

        _mockIncidenceRepository
            .Setup(r => r.Update(It.IsAny<Incidence>()))
            .Verifiable("Should NOT update");

        var result = _incidenceService.HasActiveIncidenceForAttraction(attractionId, now);

        result.Should().BeTrue();
    }

    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Maintenance")]
    public void HasActiveIncidenceForAttraction_WhenNoIncidenceOverlapsDate_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var now = new DateTime(2025, 01, 01, 10, 00, 00);

        var incidencesInDb = new List<Incidence>
        {
            new()
            {
                Id = Guid.NewGuid(),
                AttractionId = Guid.NewGuid(),
                Start = now.AddHours(-1),
                End = now.AddHours(1),
                Active = true,
                Description = "Other problem"
            },
            new()
            {
                Id = Guid.NewGuid(),
                AttractionId = attractionId,
                Start = now.AddHours(-3),
                End = now.AddHours(-2),
                Active = true,
                Description = "Old"
            },
            new()
            {
                Id = Guid.NewGuid(),
                AttractionId = attractionId,
                Start = now.AddHours(-1),
                End = now.AddHours(1),
                Active = false,
                Description = "Inactive"
            }
        };

        _mockIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Incidence, bool>>>()))
            .Returns((Expression<Func<Incidence, bool>>? filter) =>
            {
                var query = incidencesInDb.AsQueryable();
                return filter == null ? query.ToList() : query.Where(filter).ToList();
            });
        _mockIncidenceRepository
            .Setup(r => r.Update(It.IsAny<Incidence>()));

        var result = _incidenceService.HasActiveIncidenceForAttraction(attractionId, now);

        result.Should().BeFalse();
        _mockIncidenceRepository.VerifyAll();
    }
    #endregion
    #endregion
}
