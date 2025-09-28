using System.Linq.Expressions;
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
            .Setup(r => r.Add(It.IsAny<TypeIncidence>()))
            .Callback<TypeIncidence>(ti => captured = ti);

        Guid id = _typeIncidenceService.Create(_typeIncidenceArgs);

        id.Should().NotBe(Guid.Empty);
        captured.Should().NotBeNull();
        captured!.Id.Should().Be(id);
        captured.Type.Should().Be(_typeIncidenceArgs.Type);

        _mockTypeIncidenceRepository.Verify(r => r.Add(It.IsAny<TypeIncidence>()), Times.Once);
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
    public void GetAll_WhenPredicateProvided_ShouldReturnFiltered_AndCallWithPredicate()
    {
        var data = new List<TypeIncidence>
        {
            new() { Id = Guid.NewGuid(), Type = "Locked" },
            new() { Id = Guid.NewGuid(), Type = "Broken" },
            new() { Id = Guid.NewGuid(), Type = "Locked" }
        };

        _mockTypeIncidenceRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns<Expression<Func<TypeIncidence, bool>>>(pred =>
                data.Where(pred.Compile()).ToList());

        Expression<Func<TypeIncidence, bool>> predicate = t => t.Type == "Locked";

        var result = _typeIncidenceService.GetAll(predicate);

        result.Should().HaveCount(2);
        result.All(t => t.Type == "Locked").Should().BeTrue();

        _mockTypeIncidenceRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<TypeIncidence, bool>>>()),
            Times.Once);

        _mockTypeIncidenceRepository.Verify(r => r.GetAll(null), Times.Never);

        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        _mockTypeIncidenceRepository
            .Setup(r => r.GetAll(null))
            .Returns(new List<TypeIncidence>());

        var result = _typeIncidenceService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockTypeIncidenceRepository.Verify(r => r.GetAll(null), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    #endregion
    #region Get
    [TestMethod]
    public void Get_WhenEntityExists_ShouldReturnEntity()
    {
        var expected = new TypeIncidence { Id = Guid.NewGuid(), Type = "Locked" };
        Expression<Func<TypeIncidence, bool>> predicate = x => x.Type == "Locked";

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns(expected);

        var result = _typeIncidenceService.Get(predicate);

        result.Should().NotBeNull();
        result.Should().BeSameAs(expected);
        result!.Type.Should().Be("Locked");

        _mockTypeIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_WhenEntityDoesNotExist_ShouldReturnNull()
    {
        Expression<Func<TypeIncidence, bool>> predicate = x => x.Type == "Missing";

        _mockTypeIncidenceRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns((TypeIncidence?)null);

        var result = _typeIncidenceService.Get(predicate);

        result.Should().BeNull();

        _mockTypeIncidenceRepository.Verify(r => r.Get(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }
    #endregion
    #region Exist

    [TestMethod]
    public void Exist_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        Expression<Func<TypeIncidence, bool>> predicate = t => t.Type == "Locked";

        _mockTypeIncidenceRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns(true);

        var exists = _typeIncidenceService.Exist(predicate);

        exists.Should().BeTrue();
        _mockTypeIncidenceRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Exist_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        Expression<Func<TypeIncidence, bool>> predicate = t => t.Type == "Missing";

        _mockTypeIncidenceRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<TypeIncidence, bool>>>()))
            .Returns(false);

        var exists = _typeIncidenceService.Exist(predicate);

        exists.Should().BeFalse();
        _mockTypeIncidenceRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<TypeIncidence, bool>>>()), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    [TestMethod]
    public void Exist_ShouldForwardSamePredicateToRepository()
    {
        Expression<Func<TypeIncidence, bool>> predicate = t => t.Type.StartsWith("Lo");

        _mockTypeIncidenceRepository
            .Setup(r => r.Exist(It.Is<Expression<Func<TypeIncidence, bool>>>(p => p == predicate)))
            .Returns(true);

        var exists = _typeIncidenceService.Exist(predicate);

        exists.Should().BeTrue();
        _mockTypeIncidenceRepository.Verify(
            r => r.Exist(It.Is<Expression<Func<TypeIncidence, bool>>>(p => p == predicate)), Times.Once);
        _mockTypeIncidenceRepository.VerifyAll();
    }

    #endregion

}
