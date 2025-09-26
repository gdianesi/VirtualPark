using System.Linq.Expressions;
using FluentAssertions;
using Moq;
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
            .Setup(r => r.Exist(e => e.Name == _attractionArgs.Name))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Add(It.Is<Attraction>(
                a => a.Type == _attractionArgs.Type
                     && a.Name == _attractionArgs.Name
                     && a.MiniumAge == _attractionArgs.MiniumAge
                     && a.Capacity == _attractionArgs.Capacity
                     && a.Description == _attractionArgs.Description
                     && a.CurrentVisitors == _attractionArgs.CurrentVisitor
                     && a.Available == _attractionArgs.Available)));

        var attraction = _attractionService.Create(_attractionArgs);

        attraction.Should().NotBeNull();

        _mockAttractionRepository.VerifyAll();
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
}
