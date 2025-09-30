using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Models;
using VirtualPark.BusinessLogic.VisitRegistrations.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.VisitRegistrations.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("VisitRegistrationServiceTest")]
public class VisitRegistrationServiceTest
{
    private Mock<IReadOnlyRepository<VisitorProfile>> _visitorRepoMock = null!;
    private Mock<IReadOnlyRepository<Attraction>> _attractionRepoMock = null!;
    private Mock<IRepository<VisitRegistration>> _repositoryMock = null!;
    private VisitRegistrationService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        _visitorRepoMock = new Mock<IReadOnlyRepository<VisitorProfile>>(MockBehavior.Strict);
        _attractionRepoMock = new Mock<IReadOnlyRepository<Attraction>>(MockBehavior.Strict);
        _repositoryMock = new Mock<IRepository<VisitRegistration>>(MockBehavior.Strict);
        _service = new VisitRegistrationService(_repositoryMock.Object, _visitorRepoMock.Object, _attractionRepoMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldCreateVisitRegistration_WhenVisitorAndAttractionsExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var a1 = new Attraction { Name = "Roller" };
        var a2 = new Attraction { Name = "Wheel" };
        var a1Id = a1.Id;
        var a2Id = a2.Id;

        var args = new VisitRegistrationArgs(
            new List<string> { a1Id.ToString(), a2Id.ToString() },
            visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == args.VisitorProfileId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1Id))
            .Returns(a1);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a2Id))
            .Returns(a2);

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId &&
                vr.Visitor == visitor &&
                vr.Attractions.Count == 2 &&
                vr.Attractions[0].Id == a1Id &&
                vr.Attractions[1].Id == a2Id)));

        var result = _service.Create(args);

        result.Should().NotBeNull();
        result.VisitorId.Should().Be(visitorId);
        result.Visitor.Should().BeSameAs(visitor);
        result.Attractions.Should().HaveCount(2);
        result.Attractions[0].Id.Should().Be(a1Id);
        result.Attractions[1].Id.Should().Be(a2Id);

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenVisitorDoesNotExist()
    {
        var visitorId = Guid.NewGuid();
        var args = new VisitRegistrationArgs(new List<string>(), visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == args.VisitorProfileId))
            .Returns((VisitorProfile?)null);

        Action act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyNoOtherCalls();
        _repositoryMock.VerifyNoOtherCalls();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenAnyAttractionDoesNotExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var missingId = Guid.NewGuid();

        var args = new VisitRegistrationArgs(
            new List<string> { missingId.ToString() },
            visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == args.VisitorProfileId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == missingId))
            .Returns((Attraction?)null);

        var act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyNoOtherCalls();
    }
    #endregion
    #endregion
}
