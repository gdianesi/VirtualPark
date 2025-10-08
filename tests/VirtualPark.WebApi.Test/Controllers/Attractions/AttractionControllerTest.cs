using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.WebApi.Controllers.Attractions;
using VirtualPark.WebApi.Controllers.Attractions.ModelsIn;
using VirtualPark.WebApi.Controllers.Attractions.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Attractions;

[TestClass]
[TestCategory("AttractionsController")]
public class AttractionControllerTest
{
    private Mock<IAttractionService> _attractionService = null!;
    private AttractionController _attractionController = null!;
    private CreateAttractionRequest _createAttractionRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _attractionService = new Mock<IAttractionService>(MockBehavior.Strict);
        _attractionController = new AttractionController(_attractionService.Object);
        _createAttractionRequest = new CreateAttractionRequest
        {
            Name = "AttractionName",
            Type = "RollerCoaster",
            MiniumAge = "18",
            Capacity = "50",
            Description = "AttractionDescription",
            Available = "true"
        };
    }

    #region Create

    [TestMethod]
    public void Create_ValidInput_ReturnsCreatedAttractionResponse()
    {
        var returnId = Guid.NewGuid();
        var expectedArgs = _createAttractionRequest.ToArgs();

        _attractionService
            .Setup(s => s.Create(It.Is<AttractionArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Type == expectedArgs.Type &&
                a.MiniumAge == expectedArgs.MiniumAge &&
                a.Capacity == expectedArgs.Capacity &&
                a.Description == expectedArgs.Description &&
                a.Available == expectedArgs.Available)))
            .Returns(returnId);

        var response = _attractionController.CreateAttraction(_createAttractionRequest);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateAttractionResponse>();
        response.Id.Should().Be(returnId.ToString());

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void Create_ShouldWork_WhenAvailableIsFalse()
    {
        var returnId = Guid.NewGuid();
        var request = new CreateAttractionRequest
        {
            Name = "AttractionName",
            Type = "RollerCoaster",
            MiniumAge = "18",
            Capacity = "50",
            Description = "AttractionDescription",
            Available = "false"
        };

        var expectedArgs = request.ToArgs();

        _attractionService
            .Setup(s => s.Create(It.Is<AttractionArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Type == expectedArgs.Type &&
                a.MiniumAge == expectedArgs.MiniumAge &&
                a.Capacity == expectedArgs.Capacity &&
                a.Description == expectedArgs.Description &&
                a.Available == expectedArgs.Available)))
            .Returns(returnId);

        var response = _attractionController.CreateAttraction(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateAttractionResponse>();
        response.Id.Should().Be(returnId.ToString());

        _attractionService.VerifyAll();
    }

    #endregion

    #region Get
    [TestMethod]
    public void GetAttractionById_ValidInput_ReturnsGetAttractionResponse()
    {
        var ev1 = new Event { Id = Guid.NewGuid() };
        var ev2 = new Event { Id = Guid.NewGuid() };

        var attraction = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = "RollerCoaster",
            Type = AttractionType.RollerCoaster,
            MiniumAge = 18,
            Capacity = 50,
            Description = "High-speed ride",
            Available = true,
            Events = [ev1, ev2]
        };

        _attractionService
            .Setup(s => s.Get(attraction.Id))
            .Returns(attraction);

        var response = _attractionController.GetAttractionById(attraction.Id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetAttractionResponse>();

        response.Id.Should().Be(attraction.Id.ToString());
        response.Name.Should().Be("RollerCoaster");
        response.Type.Should().Be("RollerCoaster");
        response.MiniumAge.Should().Be("18");
        response.Capacity.Should().Be("50");
        response.Description.Should().Be("High-speed ride");
        response.Available.Should().Be("True");

        response.EventIds.Should().NotBeNull();
        response.EventIds!.Should().BeEquivalentTo(ev1.Id.ToString(), ev2.Id.ToString());

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void GetAttractionById_ShouldReturnResponseWithEmptyEvents_WhenAttractionHasNoEvents()
    {
        var attraction = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = "FerrisWheel",
            Type = AttractionType.Simulator,
            MiniumAge = 0,
            Capacity = 100,
            Description = "Family ride",
            Available = false,
            Events = []
        };

        _attractionService
            .Setup(s => s.Get(attraction.Id))
            .Returns(attraction);

        var response = _attractionController.GetAttractionById(attraction.Id.ToString());

        response.Should().NotBeNull();
        response.Name.Should().Be("FerrisWheel");
        response.Type.Should().Be("Simulator");
        response.MiniumAge.Should().Be("0");
        response.Capacity.Should().Be("100");
        response.Description.Should().Be("Family ride");
        response.Available.Should().Be("False");
        response.EventIds.Should().NotBeNull();
        response.EventIds!.Should().BeEmpty();

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void GetAttractionById_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";

        Action act = () => _attractionController.GetAttractionById(invalidId);

        act.Should().Throw<FormatException>();
        _attractionService.VerifyNoOtherCalls();
    }
    #endregion

    #region GetAll

    [TestMethod]
    public void GetAllAttractions_ShouldReturnMappedList()
    {
        var ev1 = new Event { Id = Guid.NewGuid() };
        var ev2 = new Event { Id = Guid.NewGuid() };

        var a1 = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = "RollerCoaster",
            Type = AttractionType.RollerCoaster,
            MiniumAge = 18,
            Capacity = 50,
            Description = "High-speed ride",
            Available = true,
            Events = [ev1, ev2]
        };

        var a2 = new Attraction
        {
            Id = Guid.NewGuid(),
            Name = "FerrisWheel",
            Type = AttractionType.Simulator,
            MiniumAge = 0,
            Capacity = 100,
            Description = "Family ride",
            Available = false,
            Events = []
        };

        var list = new List<Attraction> { a1, a2 };

        _attractionService
            .Setup(s => s.GetAll())
            .Returns(list);

        var result = _attractionController.GetAllAttractions();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Id.Should().Be(a1.Id.ToString());
        first.Name.Should().Be("RollerCoaster");
        first.Type.Should().Be("RollerCoaster");
        first.MiniumAge.Should().Be("18");
        first.Capacity.Should().Be("50");
        first.Description.Should().Be("High-speed ride");
        first.Available.Should().Be("True");
        first.EventIds.Should().BeEquivalentTo(ev1.Id.ToString(), ev2.Id.ToString());

        var second = result.Last();
        second.Id.Should().Be(a2.Id.ToString());
        second.Name.Should().Be("FerrisWheel");
        second.Type.Should().Be("Simulator");
        second.MiniumAge.Should().Be("0");
        second.Capacity.Should().Be("100");
        second.Description.Should().Be("Family ride");
        second.Available.Should().Be("False");
        second.EventIds.Should().NotBeNull();
        second.EventIds!.Should().BeEmpty();

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void GetAllAttractions_ShouldReturnEmptyList_WhenNoAttractionsExist()
    {
        _attractionService
            .Setup(s => s.GetAll())
            .Returns([]);

        var result = _attractionController.GetAllAttractions();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _attractionService.VerifyAll();
    }

    #endregion

    #region Delete

    [TestMethod]
    public void DeleteAttraction_ShouldRemoveAttraction_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _attractionService
            .Setup(s => s.Remove(id))
            .Verifiable();

        _attractionController.DeleteAttraction(id.ToString());

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void DeleteAttraction_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";

        Action act = () => _attractionController.DeleteAttraction(invalidId);

        act.Should().Throw<FormatException>();
        _attractionService.VerifyNoOtherCalls();
    }

    #endregion

    #region Update

    [TestMethod]
    public void UpdateAttraction_ShouldCallServiceUpdate_WhenDataIsValid()
    {
        var id = Guid.NewGuid();
        var request = new CreateAttractionRequest
        {
            Name = "AttractionName",
            Type = "RollerCoaster",
            MiniumAge = "18",
            Capacity = "50",
            Description = "AttractionDescription",
            Available = "true"
        };

        var expectedArgs = request.ToArgs();

        _attractionService
            .Setup(s => s.Update(It.Is<AttractionArgs>(a =>
                a.Name == expectedArgs.Name &&
                a.Type == expectedArgs.Type &&
                a.MiniumAge == expectedArgs.MiniumAge &&
                a.Capacity == expectedArgs.Capacity &&
                a.Description == expectedArgs.Description &&
                a.Available == expectedArgs.Available), id))
            .Verifiable();

        _attractionController.UpdateAttraction(id.ToString(), request);

        _attractionService.VerifyAll();
    }

    [TestMethod]
    public void UpdateAttraction_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";
        var request = new CreateAttractionRequest
        {
            Name = "AttractionName",
            Type = "RollerCoaster",
            MiniumAge = "18",
            Capacity = "50",
            Description = "AttractionDescription",
            Available = "true"
        };

        Action act = () => _attractionController.UpdateAttraction(invalidId, request);

        act.Should().Throw<FormatException>();
        _attractionService.VerifyNoOtherCalls();
    }

    #endregion

    #region Report
    [TestMethod]
    public void GetAttractionsReport_ValidInput_ReturnsMappedList()
    {
        var from = "2025-10-01";
        var to = "2025-10-31";

        var raw = new List<string>
        {
            "Montaña Rusa\t423",
            "Simulador B\t822"
        };

        _attractionService
            .Setup(s => s.AttractionsReport(DateTime.Parse(from), DateTime.Parse(to)))
            .Returns(raw);

        var result = _attractionController.GetAttractionsReport(from, to);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Name.Should().Be("Montaña Rusa");
        result[0].Visits.Should().Be("423");

        result[1].Name.Should().Be("Simulador B");
        result[1].Visits.Should().Be("822");

        _attractionService.VerifyAll();
    }
    #endregion
}
