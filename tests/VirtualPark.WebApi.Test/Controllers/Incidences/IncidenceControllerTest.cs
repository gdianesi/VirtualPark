using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Models;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.WebApi.Controllers.Incidences;
using VirtualPark.WebApi.Controllers.Incidences.ModelsIn;
using VirtualPark.WebApi.Controllers.Incidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Incidences;

[TestClass]
public class IncidenceControllerTest
{
    private Mock<IIncidenceService> _incidenceServiceMock = null!;
    private IncidenceController _incidencesController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _incidenceServiceMock = new Mock<IIncidenceService>(MockBehavior.Strict);
        _incidencesController = new IncidenceController(_incidenceServiceMock.Object);
    }

    #region Create
    [TestMethod]
        public void CreateIncidence_ValidInput_ReturnsCreatedIncidenceResponse()
        {
            var typeId = Guid.NewGuid().ToString();
            var attractionId = Guid.NewGuid().ToString();
            var returnId = Guid.NewGuid();

            var request = new CreateIncidenceRequest
            {
                TypeId = typeId,
                Description = "Falla técnica en atracción",
                Start = "2025-10-06 10:00:00",
                End = "2025-10-06 11:00:00",
                AttractionId = attractionId,
                Active = "true"
            };

            var expectedArgs = request.ToArgs();

            _incidenceServiceMock
                .Setup(s => s.Create(It.Is<IncidenceArgs>(a =>
                    a.TypeIncidence == expectedArgs.TypeIncidence &&
                    a.Description == expectedArgs.Description &&
                    a.Start == expectedArgs.Start &&
                    a.End == expectedArgs.End &&
                    a.AttractionId == expectedArgs.AttractionId &&
                    a.Active == expectedArgs.Active)))
                .Returns(returnId);

            var response = _incidencesController.CreateIncidence(request);

            response.Should().NotBeNull();
            response.Should().BeOfType<CreateIncidenceResponse>();
            response.Id.Should().Be(returnId.ToString());

            _incidenceServiceMock.VerifyAll();
        }

        [TestMethod]
        public void CreateIncidence_ShouldWork_WithDateOnlyAndInactive()
        {
            var typeId = Guid.NewGuid().ToString();
            var attractionId = Guid.NewGuid().ToString();
            var returnId = Guid.NewGuid();

            var request = new CreateIncidenceRequest
            {
                TypeId = typeId,
                Description = "Incidencia general",
                Start = "2025-10-06",
                End = "2025-10-06",
                AttractionId = attractionId,
                Active = "false"
            };

            var expectedArgs = request.ToArgs();

            _incidenceServiceMock
                .Setup(s => s.Create(It.Is<IncidenceArgs>(a =>
                    a.TypeIncidence == expectedArgs.TypeIncidence &&
                    a.Description == expectedArgs.Description &&
                    a.Start == expectedArgs.Start &&
                    a.End == expectedArgs.End &&
                    a.AttractionId == expectedArgs.AttractionId &&
                    a.Active == expectedArgs.Active)))
                .Returns(returnId);

            var response = _incidencesController.CreateIncidence(request);

            response.Should().NotBeNull();
            response.Id.Should().Be(returnId.ToString());

            _incidenceServiceMock.VerifyAll();
        }
    #endregion
     #region Get
        [TestMethod]
        public void GetIncidence_ValidInput_ReturnsGetIncidenceResponse()
        {
            var incidenceId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var attractionId = Guid.NewGuid();

            var incidence = new Incidence
            {
                Id = incidenceId,
                Type = new TypeIncidence() { Id = typeId, Type = "Técnica" },
                Description = "Falla técnica",
                Start = new DateTime(2025, 10, 6, 10, 0, 0),
                End = new DateTime(2025, 10, 6, 11, 0, 0),
                AttractionId = attractionId,
                Active = true
            };

            _incidenceServiceMock
                .Setup(s => s.Get(incidenceId))
                .Returns(incidence);

            var response = _incidencesController.GetIncidence(incidenceId.ToString());

            response.Should().NotBeNull();
            response.Should().BeOfType<GetIncidenceResponse>();
            response.Id.Should().Be(incidenceId.ToString());
            response.TypeId.Should().Be(typeId.ToString());
            response.Description.Should().Be("Falla técnica");
            response.Start.Should().Be(incidence.Start.ToString());
            response.End.Should().Be(incidence.End.ToString());
            response.AttractionId.Should().Be(attractionId.ToString());
            response.Active.Should().Be(true.ToString());

            _incidenceServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetIncidence_ShouldWork_WithInactiveAndDateOnly()
        {
            var incidenceId = Guid.NewGuid();
            var typeId = Guid.NewGuid();
            var attractionId = Guid.NewGuid();

            var incidence = new Incidence
            {
                Id = incidenceId,
                Type = new TypeIncidence() { Id = typeId, Type = "General" },
                Description = "Corte de energía",
                Start = new DateTime(2025, 10, 6),
                End = new DateTime(2025, 10, 6),
                AttractionId = attractionId,
                Active = false
            };

            _incidenceServiceMock
                .Setup(s => s.Get(incidenceId))
                .Returns(incidence);

            var response = _incidencesController.GetIncidence(incidenceId.ToString());

            response.Should().NotBeNull();
            response.Id.Should().Be(incidenceId.ToString());
            response.TypeId.Should().Be(typeId.ToString());
            response.Description.Should().Be("Corte de energía");
            response.Start.Should().Be(incidence.Start.ToString());
            response.End.Should().Be(incidence.End.ToString());
            response.AttractionId.Should().Be(attractionId.ToString());
            response.Active.Should().Be(false.ToString());

            _incidenceServiceMock.VerifyAll();
        }

        [TestMethod]
        public void GetIncidence_ShouldThrow_WhenIdIsInvalid()
        {
            var invalidId = "not-a-guid";

            Action act = () => _incidencesController.GetIncidence(invalidId);

            act.Should().Throw<FormatException>();
            _incidenceServiceMock.VerifyNoOtherCalls();
        }
        #endregion
}
