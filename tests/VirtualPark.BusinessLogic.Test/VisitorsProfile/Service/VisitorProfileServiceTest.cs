using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("VisitorProfileServiceTest")]
public class VisitorProfileServiceTest
{
    private Mock<IRepository<VisitorProfile>> _repositoryMock = null!;
    private VisitorProfileService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repositoryMock = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);
        _service = new VisitorProfileService(_repositoryMock.Object);
    }

    #region Create
    [TestMethod]
    [TestCategory("Validation")]
    public void CreateVisitorProfile_ShouldCreate_WhenArgsAreValid()
    {
        var args = new VisitorProfileArgs("2002-07-30", "Standard");

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitorProfile>(v =>
                v.DateOfBirth == args.DateOfBirth &&
                v.Membership == args.Membership)));

        var visitorProfile = _service.Create(args);

        visitorProfile.Id.Should().NotBeEmpty();
        _repositoryMock.VerifyAll();
    }
    #endregion
}
