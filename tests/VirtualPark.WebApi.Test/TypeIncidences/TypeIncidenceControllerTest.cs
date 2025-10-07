using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.TypeIncidences.Entity;
using VirtualPark.BusinessLogic.TypeIncidences.Models;
using VirtualPark.BusinessLogic.TypeIncidences.Service;
using VirtualPark.WebApi.TypeIncidences;
using VirtualPark.WebApi.TypeIncidences.ModelsIn;
using VirtualPark.WebApi.TypeIncidences.ModelsOut;

namespace VirtualPark.WebApi.Test.TypeIncidences;

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
}
