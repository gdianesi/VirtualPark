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
    public void Create_WhenArgsAreValid_ShouldReturnNewGuid_AndAddEntityWithSameId()
    {
        TypeIncidence? captured = null;

        _mockTypeIncidenceRepository
            .Setup(r => r.Add(It.Is<TypeIncidence>(ti => ti.Type == _typeIncidenceArgs.Type)))
            .Callback<TypeIncidence>(ti => captured = ti);

        Guid id = _typeIncidenceService.Create(_typeIncidenceArgs);

        id.Should().NotBe(Guid.Empty);
        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);
        captured.Type.Should().Be(_typeIncidenceArgs.Type);

        _mockTypeIncidenceRepository.VerifyAll();
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

    #region GetAll
    [TestMethod]
    public void GetAll_WhenRepositoryReturnsData_ShouldReturnList()
    {
        var data = new List<TypeIncidence>
        {
            new() { Id = Guid.NewGuid(), Type = "Locked" },
            new() { Id = Guid.NewGuid(), Type = "Broken" },
            new() { Id = Guid.NewGuid(), Type = "Locked" }
        };

        _mockTypeIncidenceRepository
            .Setup(r => r.GetAll(null))
            .Returns(data);

        var result = _typeIncidenceService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(3);

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        _mockTypeIncidenceRepository
            .Setup(r => r.GetAll(null))
            .Returns([]);

        var result = _typeIncidenceService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockTypeIncidenceRepository.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void Get_WhenEntityExists_ShouldReturnEntity()
    {
        var expected = new TypeIncidence { Id = Guid.NewGuid(), Type = "Locked" };
        var id = expected.Id;

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns(expected);

        var result = _typeIncidenceService.Get(expected.Id);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);
        result!.Type.Should().Be("Locked");

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_WhenEntityDoesNotExist_ShouldReturnNull()
    {
        var missingId = Guid.NewGuid();

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == missingId))
            .Returns((TypeIncidence?)null);

        var act = () => _typeIncidenceService.Get(missingId);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Type incidence don't exist");

        _mockTypeIncidenceRepository.VerifyAll();
    }
    #endregion

    #region ApplyArgsToEntity
    [TestMethod]
    public void ApplyArgsToEntity_ShouldCopyTypeFromArgs()
    {
        var entity = new TypeIncidence { Id = Guid.NewGuid(), Type = "OldValue" };
        var args = new TypeIncidenceArgs(type: "NewValue");

        TypeIncidenceService.ApplyArgsToEntity(entity, args);

        entity.Type.Should().Be("NewValue");
    }

    #endregion

    #region Update

    [TestMethod]
    public void Update_WhenEntityExists_ShouldApplyArgsAndCallRepositoryUpdate()
    {
        var id = Guid.NewGuid();
        var existing = new TypeIncidence { Id = id, Type = "OldType" };
        var args = new TypeIncidenceArgs("NewType");

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns(existing);

        _mockTypeIncidenceRepository
            .Setup(r => r.Update(It.Is<TypeIncidence>(ti => ti.Id == id && ti.Type == "NewType")));

        _typeIncidenceService.Update(id, args);

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_WhenEntityDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();
        var args = new TypeIncidenceArgs("NewType");

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns((TypeIncidence?)null);

        var act = () => _typeIncidenceService.Update(id, args);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Type incidence don't exist");

        _mockTypeIncidenceRepository.VerifyAll();
    }

    #endregion

    #region Delete
    [TestMethod]
    public void Delete_WhenEntityExists_ShouldCallRepositoryRemove()
    {
        var id = Guid.NewGuid();
        var existing = new TypeIncidence { Id = id, Type = "Locked" };

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns(existing);

        _mockTypeIncidenceRepository
            .Setup(r => r.Remove(existing));

        _typeIncidenceService.Delete(id);

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Delete_WhenEntityDoesNotExist_ShouldThrowInvalidOperationException()
    {
        var id = Guid.NewGuid();

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(t => t.Id == id))
            .Returns((TypeIncidence?)null);

        Action act = () => _typeIncidenceService.Delete(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"TypeIncidence with id {id} not found.");

        _mockTypeIncidenceRepository.VerifyAll();
    }
    #endregion
}
