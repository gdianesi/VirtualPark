using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.AttractionsEvents.Entity;
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
                Events = new List<Event> { ev1, ev2 }
            };

            _attractionService
                .Setup(s => s.Get(attraction.Id))
                .Returns(attraction);

            var response = _attractionController.GetAttractionById(attraction.Id.ToString());

            response.Should().NotBeNull();
            response.Should().BeOfType<GetAttractionResponse>();

            response.Id.Should().Be(attraction.Id.ToString());
            response.Name.Should().Be("RollerCoaster");
            response.Type.Should().Be("RollerCoaster");   // enum .ToString()
            response.MiniumAge.Should().Be("18");          // int .ToString()
            response.Capacity.Should().Be("50");           // int .ToString()
            response.Description.Should().Be("High-speed ride");
            response.Available.Should().Be("True");        // bool .ToString() => "True"/"False"

            response.EventIds.Should().NotBeNull();
            response.EventIds!.Should().BeEquivalentTo(new[]
            {
                ev1.Id.ToString(),
                ev2.Id.ToString()
            });

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
                Events = new List<Event>()
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
    }
