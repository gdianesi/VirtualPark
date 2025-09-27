using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Attractions.Services;

[TestClass]
[TestCategory("Attractions")]
[TestCategory("AttractionServic")]
public class AttractionServiceTest
{
    private Mock<IRepository<Attraction>> _mockAttractionRepository = null!;
    private AttractionService _attractionService = null!;
    private AttractionArgs _attractionArgs = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockAttractionRepository = new Mock<IRepository<Attraction>>(MockBehavior.Strict);
        _attractionService = new AttractionService(_mockAttractionRepository.Object);
        _attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true");
    }

    #region create
    [TestMethod]
    public void Create_WhenArgsAreValid_ShouldCreateAttraction()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Add(It.IsAny<Attraction>()));

        var attraction = _attractionService.Create(_attractionArgs);

        attraction.Should().NotBeNull();
        attraction.Name.Should().Be(_attractionArgs.Name);

        _mockAttractionRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()));

        _mockAttractionRepository.Verify(
            r => r.Add(It.Is<Attraction>(a =>
                a.Name == _attractionArgs.Name &&
                a.Type == _attractionArgs.Type &&
                a.MiniumAge == _attractionArgs.MiniumAge &&
                a.Capacity == _attractionArgs.Capacity &&
                a.Description == _attractionArgs.Description &&
                a.CurrentVisitors == _attractionArgs.CurrentVisitor &&
                a.Available == _attractionArgs.Available)),
            Times.Once);
    }

    #endregion
    #region validationName
    [TestCategory("Validation")]
    [TestMethod]
    public void Create_WhenNameIsEmpty_ShouldThrowException()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(true);

        Assert.ThrowsException<ArgumentException>(
            () => _attractionService.ValidateAttractionName(string.Empty));
    }

    [TestMethod]
    public void Create_WhenNameAlreadyExists_ShouldThrowDuplicateException()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(true);

        Action act = () => _attractionService.Create(_attractionArgs);

        act.Should().Throw<Exception>();

        _mockAttractionRepository.Verify(r => r.Add(It.IsAny<Attraction>()), Times.Never);
    }

    [TestMethod]
    public void Create_WhenNameIsValid_ShouldCreate()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Add(It.IsAny<Attraction>()));

        Action act = () => _attractionService.Create(_attractionArgs);

        act.Should().NotThrow();
        _mockAttractionRepository.Verify(r => r.Add(It.IsAny<Attraction>()), Times.Once);
    }

    #endregion
    #region MapToEntity

    [TestMethod]
    [TestCategory("Validation")]
    public void MapToEntity_WhenArgsAreValid_ShouldReturnAttractionEntity()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        var attraction = _attractionService.MapToEntity(_attractionArgs);

        attraction.Should().NotBeNull();
        attraction.Should().BeEquivalentTo(_attractionArgs, opt => opt
            .ExcludingMissingMembers());
    }

    #endregion
    #region getAll

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenAttractionsExist_ShouldReturnAllAttractions()
    {
        var attractions = new List<Attraction>
        {
            new Attraction { Name = "RollerCoaster", Type = AttractionType.RollerCoaster, Capacity = 50 },
            new Attraction { Name = "FerrisWheel",  Type = AttractionType.Simulator,  Capacity = 100 }
        };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(attractions);

        var result = _attractionService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.Name == "RollerCoaster");
        result.Should().Contain(a => a.Name == "FerrisWheel");

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenNoAttractionsExist_ShouldReturnEmptyList()
    {
        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns([]);

        var result = _attractionService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WithPredicate_ShouldReturnFilteredAttractions()
    {
        Expression<Func<Attraction, bool>> predicate = a => a.Capacity > 60;

        var filtered = new List<Attraction>
        {
            new Attraction { Name = "FerrisWheel", Type = AttractionType.Simulator, Capacity = 100 }
        };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(filtered);

        var result = _attractionService.GetAll(predicate);

        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("FerrisWheel");

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()), Times.Once);
        _mockAttractionRepository.Verify(r => r.GetAll(null), Times.Never);
    }

    #endregion
    #region Get

    [TestMethod]
    public void Get_WhenAttractionExists_ShouldReturnAttraction()
    {
        var expected = new Attraction { Name = "RollerCoaster", Capacity = 50 };

        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(expected);

        var result = _attractionService.Get(a => a.Name == "RollerCoaster");

        result.Should().NotBeNull();
        result!.Name.Should().Be("RollerCoaster");
        result.Capacity.Should().Be(50);

        _mockAttractionRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    public void Get_WhenAttractionDoesNotExist_ShouldReturnNull()
    {
        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Attraction?)null);

        var result = _attractionService.Get(a => a.Name == "GhostTrain");

        result.Should().BeNull();

        _mockAttractionRepository.Verify(
            r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }
    #endregion
    #region Exist

    [TestMethod]
    [TestCategory("Validation")]
    public void Exist_WhenAttractionExists_ShouldReturnTrue()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(true);

        var result = _attractionService.Exist(a => a.Name == "RollerCoaster");

        result.Should().BeTrue();

        _mockAttractionRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Exist_WhenAttractionDoesNotExist_ShouldReturnFalse()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        var result = _attractionService.Exist(a => a.Name == "GhostTrain");

        result.Should().BeFalse();

        _mockAttractionRepository.Verify(
            r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }
    #endregion
    #region Update

    [TestMethod]
    [TestCategory("Validation")]
    public void ApplyArgsToEntity_ShouldCopyAllPropertiesFromArgs()
    {
        var entity = new Attraction();

        AttractionService.ApplyArgsToEntity(entity, _attractionArgs);

        entity.Name.Should().Be(_attractionArgs.Name);
        entity.Type.Should().Be(_attractionArgs.Type);
        entity.MiniumAge.Should().Be(_attractionArgs.MiniumAge);
        entity.Capacity.Should().Be(_attractionArgs.Capacity);
        entity.Description.Should().Be(_attractionArgs.Description);
        entity.CurrentVisitors.Should().Be(_attractionArgs.CurrentVisitor);
        entity.Available.Should().Be(_attractionArgs.Available);
    }
    #endregion
    #region Update
    [TestMethod]
    [TestCategory("Validations")]
    public void Update_ShouldCopyAllPropertiesFromArgs_AndPersist()
    {
        var id = Guid.NewGuid();

        var existing = new Attraction
        {
            Id = id,
            Name = "Old name",
            Type = AttractionType.RollerCoaster,
            MiniumAge = 10,
            Capacity = 100,
            Description = "Old desc",
            CurrentVisitors = 0,
            Available = false
        };

        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(existing);

        Attraction? updated = null;
        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()))
            .Callback<Attraction>(a => updated = a);

        var args = new AttractionArgs(
            type: "Simulator",
            name: "Example Attraction",
            miniumAge: "13",
            capacity: "500",
            description: "New description",
            currentVisitor: "50",
            available: "true");

        _attractionService.Update(args, id);

        updated.Should().NotBeNull();
        updated!.Id.Should().Be(id);
        updated.Name.Should().Be("Example Attraction");
        updated.Type.Should().Be(AttractionType.Simulator);
        updated.MiniumAge.Should().Be(13);
        updated.Capacity.Should().Be(500);
        updated.Description.Should().Be("New description");
        updated.CurrentVisitors.Should().Be(50);
        updated.Available.Should().BeTrue();

        _mockAttractionRepository.Verify(r => r.Update(It.IsAny<Attraction>()), Times.Once);
        _mockAttractionRepository.VerifyAll();
    }
    #endregion
    #region Remove
    [TestMethod]
    public void Remove_WhenAttractionExists_ShouldRemoveOnce()
    {
        var id = Guid.NewGuid();
        var existing = new Attraction { Id = id, Name = "To Remove" };

        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(existing);

        Attraction? removed = null;
        _mockAttractionRepository
            .Setup(r => r.Remove(It.IsAny<Attraction>()))
            .Callback<Attraction>(a => removed = a);

        _attractionService.Remove(id);

        removed.Should().NotBeNull();
        removed!.Id.Should().Be(id);

        _mockAttractionRepository.Verify(r => r.Remove(It.Is<Attraction>(a => a.Id == id)), Times.Once);
        _mockAttractionRepository.VerifyAll();
    }

    [TestMethod]
    public void Remove_WhenAttractionDoesNotExist_ShouldThrow_AndNotCallRemove()
    {
        var id = Guid.NewGuid();

        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Attraction?)null);

        Action act = () => _attractionService.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Attraction with id {id} not found.");

        _mockAttractionRepository.Verify(r => r.Remove(It.IsAny<Attraction>()), Times.Never);
        _mockAttractionRepository.VerifyAll();
    }
    #endregion
}
