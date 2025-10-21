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

    #region GetAll
    [TestMethod]
    public void GetAllVisitorProfiles_ShouldReturnMappedList()
    {
        var vp1 = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2002, 07, 30),
            Membership = Membership.Standard,
            Score = 85,
        };

        var nfcId = vp1.NfcId;

        var vp2 = new VisitorProfile
        {
            DateOfBirth = new DateOnly(1998, 07, 30),
            Membership = Membership.Premium,
            Score = 99,
        };

        var nfcId2 = vp2.NfcId;

        var list = new List<VisitorProfile> { vp1, vp2 };

        _serviceMock
            .Setup(s => s.GetAll())
            .Returns(list);

        var result = _controller.GetAllVisitorProfiles();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Id.Should().Be(vp1.Id.ToString());
        first.DateOfBirth.Should().Be("2002-07-30");
        first.Membership.Should().Be("Standard");
        first.Score.Should().Be("85");
        first.NfcId.Should().Be(nfcId.ToString());

        var second = result.Last();
        second.Id.Should().Be(vp2.Id.ToString());
        second.DateOfBirth.Should().Be("1998-07-30");
        second.Membership.Should().Be("Premium");
        second.Score.Should().Be("99");
        second.NfcId.Should().Be(nfcId2.ToString());

        _serviceMock.VerifyAll();
    }
    #endregion
}
