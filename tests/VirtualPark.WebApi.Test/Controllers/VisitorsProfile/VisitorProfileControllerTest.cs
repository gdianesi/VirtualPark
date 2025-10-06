using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.WebApi.Controllers.VisitorsProfile;
using VirtualPark.WebApi.Controllers.VisitorsProfile.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.VisitorsProfile;

[TestClass]
public class VisitorProfileControllerTest
{
    private Mock<IVisitorProfileService> _serviceMock = null!;
    private VisitorProfileController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _serviceMock = new Mock<IVisitorProfileService>(MockBehavior.Strict);
        _controller = new VisitorProfileController(_serviceMock.Object);
    }

    #region Get
    [TestMethod]
    public void GetVisitorProfileById_ValidInput_ReturnsResponse()
    {
        var vp = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2002, 07, 30),
            Membership = Membership.Premium,
            Score = 97,
        };
        var nfcId = vp.NfcId;
        var id = vp.Id;

        _serviceMock
            .Setup(s => s.Get(id))
            .Returns(vp);

        var result = _controller.GetVisitorProfileById(id.ToString());

        result.Should().NotBeNull();
        result.Should().BeOfType<GetVisitorProfileResponse>();
        result.Id.Should().Be(id.ToString());
        result.DateOfBirth.Should().Be("2002-07-30");
        result.Membership.Should().Be("Premium");
        result.Score.Should().Be("97");
        result.NfcId.Should().Be(nfcId.ToString());

        _serviceMock.VerifyAll();
    }
    #endregion
}
