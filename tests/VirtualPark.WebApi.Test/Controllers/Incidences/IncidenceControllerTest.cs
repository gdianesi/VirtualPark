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
    #region GetAll
    [TestMethod]
    public void GetAllIncidences_ShouldReturnMappedList()
    {
        var type1 = new TypeIncidence() { Id = Guid.NewGuid(), Type = "Técnica" };
        var type2 = new TypeIncidence { Id = Guid.NewGuid(), Type = "General" };

        var inc1 = new Incidence
        {
            Id = Guid.NewGuid(),
            Type = type1,
            Description = "Falla en motor",
            Start = new DateTime(2025, 10, 6, 10, 0, 0),
            End = new DateTime(2025, 10, 6, 11, 0, 0),
            AttractionId = Guid.NewGuid(),
            Active = true
        };

        var inc2 = new Incidence
        {
            Id = Guid.NewGuid(),
            Type = type2,
            Description = "Corte de energía",
            Start = new DateTime(2025, 10, 7, 12, 0, 0),
            End = new DateTime(2025, 10, 7, 13, 0, 0),
            AttractionId = Guid.NewGuid(),
            Active = false
        };

        var incidences = new List<Incidence> { inc1, inc2 };

        _incidenceServiceMock
            .Setup(s => s.GetAll())
            .Returns(incidences);

        var result = _incidencesController.GetAllIncidences();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Id.Should().Be(inc1.Id.ToString());
        first.TypeId.Should().Be(inc1.Type.Id.ToString());
        first.Description.Should().Be("Falla en motor");
        first.Start.Should().Be(inc1.Start.ToString());
        first.End.Should().Be(inc1.End.ToString());
        first.AttractionId.Should().Be(inc1.AttractionId.ToString());
        first.Active.Should().Be(true.ToString());

        var second = result.Last();
        second.Id.Should().Be(inc2.Id.ToString());
        second.TypeId.Should().Be(inc2.Type.Id.ToString());
        second.Description.Should().Be("Corte de energía");
        second.Start.Should().Be(inc2.Start.ToString());
        second.End.Should().Be(inc2.End.ToString());
        second.AttractionId.Should().Be(inc2.AttractionId.ToString());
        second.Active.Should().Be(false.ToString());

        _incidenceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void GetAllIncidences_ShouldReturnEmptyList_WhenNoIncidencesExist()
    {
        _incidenceServiceMock
            .Setup(s => s.GetAll())
            .Returns([]);

        var result = _incidencesController.GetAllIncidences();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _incidenceServiceMock.VerifyAll();
    }
    #endregion
    #region Delete
    [TestMethod]
    public void DeleteIncidence_ShouldRemoveIncidence_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _incidenceServiceMock
            .Setup(s => s.Remove(id))
            .Verifiable();

        _incidencesController.DeleteIncidence(id.ToString());

        _incidenceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void DeleteIncidence_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";

        Action act = () => _incidencesController.DeleteIncidence(invalidId);

        act.Should().Throw<FormatException>();
        _incidenceServiceMock.VerifyNoOtherCalls();
    }
    #endregion
    #region Put
    [TestMethod]
    public void UpdateIncidence_ShouldCallServiceUpdate_WhenDataIsValid()
    {
        var id = Guid.NewGuid();
        var typeId = Guid.NewGuid().ToString();
        var attractionId = Guid.NewGuid().ToString();

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
            .Setup(s => s.Update(It.Is<IncidenceArgs>(a =>
                a.TypeIncidence == expectedArgs.TypeIncidence &&
                a.Description == expectedArgs.Description &&
                a.Start == expectedArgs.Start &&
                a.End == expectedArgs.End &&
                a.AttractionId == expectedArgs.AttractionId &&
                a.Active == expectedArgs.Active), id))
            .Verifiable();

        _incidencesController.UpdateIncidence(id.ToString(), request);

        _incidenceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void UpdateIncidence_ShouldThrow_WhenIdIsInvalid()
    {
        var invalidId = "not-a-guid";
        var request = new CreateIncidenceRequest
        {
            TypeId = Guid.NewGuid().ToString(),
            Description = "Falla técnica",
            Start = "2025-10-06 10:00:00",
            End = "2025-10-06 11:00:00",
            AttractionId = Guid.NewGuid().ToString(),
            Active = "true"
        };

        Action act = () => _incidencesController.UpdateIncidence(invalidId, request);

        act.Should().Throw<FormatException>();
        _incidenceServiceMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void UpdateIncidence_ShouldCallServiceUpdate_WithDateOnly_AndInactive()
    {
        var id = Guid.NewGuid();
        var typeId = Guid.NewGuid().ToString();
        var attractionId = Guid.NewGuid().ToString();

        var request = new CreateIncidenceRequest
        {
            TypeId = typeId,
            Description = "Mantenimiento programado",
            Start = "2025-10-06",
            End = "2025-10-06",
            AttractionId = attractionId,
            Active = "false"
        };

        var expectedArgs = request.ToArgs();

        _incidenceServiceMock
            .Setup(s => s.Update(It.Is<IncidenceArgs>(a =>
                    a.TypeIncidence == expectedArgs.TypeIncidence &&
                    a.Description == expectedArgs.Description &&
                    a.Start == expectedArgs.Start &&
                    a.End == expectedArgs.End &&
                    a.AttractionId == expectedArgs.AttractionId &&
                    a.Active == expectedArgs.Active),
                id))
            .Verifiable();

        _incidencesController.UpdateIncidence(id.ToString(), request);

        _incidenceServiceMock.VerifyAll();
    }

    [TestMethod]
    public void CreateIncidence_ShouldWork_WithIso8601TDateTime()
    {
        var typeId = Guid.NewGuid().ToString();
        var attractionId = Guid.NewGuid().ToString();
        var returnId = Guid.NewGuid();

        var request = new CreateIncidenceRequest
        {
            TypeId = typeId,
            Description = "Falla intermitente",
            Start = "2025-10-06T10:00:00",
            End = "2025-10-06T11:00:00",
            AttractionId = attractionId,
            Active = "true"
        };

        var expected = request.ToArgs();

        _incidenceServiceMock
            .Setup(s => s.Create(It.Is<IncidenceArgs>(a =>
                a.TypeIncidence == expected.TypeIncidence &&
                a.Description == expected.Description &&
                a.Start == expected.Start &&
                a.End == expected.End &&
                a.AttractionId == expected.AttractionId &&
                a.Active == expected.Active)))
            .Returns(returnId);

        var res = _incidencesController.CreateIncidence(request);

        res.Id.Should().Be(returnId.ToString());
        _incidenceServiceMock.VerifyAll();
    }
    #endregion
}
