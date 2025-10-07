using FluentAssertions;
using Moq;
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

        // Act
        var response = _controller.CreateTypeIncidence(request);

        // Assert
        response.Should().NotBeNull();
        response.Should().BeOfType<CreateTypeIncidenceResponse>();
        response.Id.Should().Be(returnId.ToString());

        _serviceMock.VerifyAll();
    }
}
