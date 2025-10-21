using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.WebApi.Controllers.TypeIncidences;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsIn;
using VirtualPark.WebApi.Controllers.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.TypeIncidences;

[TestClass]
public class TypeIncidenceControllerTest
{
    private Mock<ITypeIncidenceService> _serviceMock = null!;
    private TypeIncidenceController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _serviceMock = new Mock<ITypeIncidenceService>(MockBehavior.Strict);
        _controller = new TypeIncidenceController(_serviceMock.Object);
    }

    #region Create
    [TestMethod]
    public void CreateTypeIncidence_ValidInput_ReturnsCreatedResponse()
    {
        var request = new CreateTypeIncidenceRequest
        {
            Type = "type"
        };

        var expectedArgs = new TypeIncidenceArgs("type");
        var returnId = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.Create(It.Is<TypeIncidenceArgs>(a => a.Type == expectedArgs.Type)))
            .Returns(returnId);

        var response = _controller.CreateTypeIncidence(request);

        response.Should().NotBeNull();
        response.Should().BeOfType<CreateTypeIncidenceResponse>();
        response.Id.Should().Be(returnId.ToString());

        _serviceMock.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void GetTypeIncidenceById_ValidInput_ReturnsResponse()
    {
        var entity = new TypeIncidence { Type = "type" };
        var id = entity.Id;

        _serviceMock
            .Setup(s => s.Get(id))
            .Returns(entity);

        var response = _controller.GetTypeIncidenceById(id.ToString());

        response.Should().NotBeNull();
        response.Should().BeOfType<GetTypeIncidenceResponse>();
        response.Id.Should().Be(id.ToString());
        response.Type.Should().Be("type");

        _serviceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAllTypeIncidences_ShouldReturnMappedList()
    {
        var t1 = new TypeIncidence { Type = "Mechanical" };
        var t2 = new TypeIncidence { Type = "Electrical" };
        var list = new List<TypeIncidence> { t1, t2 };

        _serviceMock
            .Setup(s => s.GetAll())
            .Returns(list);

        var result = _controller.GetAllTypeIncidences();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Should().BeOfType<GetTypeIncidenceResponse>();
        first.Id.Should().Be(t1.Id.ToString());
        first.Type.Should().Be("Mechanical");

        var second = result.Last();
        second.Id.Should().Be(t2.Id.ToString());
        second.Type.Should().Be("Electrical");

        _serviceMock.VerifyAll();
    }
    #endregion

    #region Delete
    [TestMethod]
    public void DeleteTypeIncidence_ShouldCallServiceDelete_WhenIdIsValid()
    {
        var id = Guid.NewGuid();

        _serviceMock
            .Setup(s => s.Delete(id));

        _controller.DeleteTypeIncidence(id.ToString());

        _serviceMock.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void UpdateTypeIncidence_ShouldCallServiceUpdate_WhenInputIsValid()
    {
        var id = Guid.NewGuid();
        var request = new CreateTypeIncidenceRequest { Type = "type" };
        var expectedArgs = new TypeIncidenceArgs("type");

        _serviceMock
            .Setup(s => s.Update(
                id,
                It.Is<TypeIncidenceArgs>(a => a.Type == expectedArgs.Type)));

        _controller.UpdateTypeIncidence(id.ToString(), request);

        _serviceMock.VerifyAll();
    }
    #endregion
}
