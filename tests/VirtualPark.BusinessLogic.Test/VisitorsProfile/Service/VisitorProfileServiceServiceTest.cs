using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Service;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.VisitorsProfile.Service;

[TestClass]
[TestCategory("Service")]
[TestCategory("VisitorProfileServiceServiceTest")]
public class VisitorProfileServiceServiceTest
{
    private Mock<IRepository<VisitorProfile>> _repositoryMock = null!;
    private VisitorProfileServiceService _serviceService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repositoryMock = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);
        _serviceService = new VisitorProfileServiceService(_repositoryMock.Object);
    }

    #region Create
    [TestMethod]
    [TestCategory("Validation")]
    public void CreateVisitorProfile_ShouldCreate_WhenArgsAreValid()
    {
        var args = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitorProfile>(v =>
                v.DateOfBirth == args.DateOfBirth &&
                v.Membership == args.Membership)));

        var visitorProfile = _serviceService.Create(args);

        visitorProfile.Id.Should().NotBeEmpty();
        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Remove
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void RemoveVisitorProfile_ShouldRemove_WhenVisitorExists()
    {
        var visitor = new VisitorProfile();
        Guid? id = visitor.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(visitor);

        _repositoryMock
            .Setup(r => r.Remove(visitor));

        _serviceService.Remove(id);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void RemoveVisitorProfile_ShouldThrow_WhenVisitorDoesNotExist()
    {
        Guid? id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitorProfile?)null);

        Action act = () => _serviceService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");
        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetVisitorProfile_ShouldReturn_WhenVisitorExists()
    {
        var expected = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard,
            Score = 85
        };
        var id = expected.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(expected);

        var result = _serviceService.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.DateOfBirth.Should().Be(new DateOnly(2000, 1, 1));
        result.Membership.Should().Be(Membership.Standard);
        result.Score.Should().Be(85);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetVisitorProfile_ShouldThrow_WhenVisitorDoesNotExist()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitorProfile?)null);

        var act = () => _serviceService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Update
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void UpdateVisitorProfile_ShouldApplyChanges_AndPersist_WhenVisitorExists()
    {
        var existing = new VisitorProfile
        {
            DateOfBirth = new DateOnly(1990, 1, 1),
            Membership = Membership.Standard,
            Score = 10
        };
        var id = existing.Id;

        var args = new VisitorProfileArgs("2002-07-30", "Premium", "85");

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(existing);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitorProfile>(v =>
                v.Id == id &&
                v.DateOfBirth == args.DateOfBirth &&
                v.Membership == args.Membership &&
                v.Score == args.Score)));

        _serviceService.Update(args, id);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void UpdateVisitorProfile_ShouldThrow_WhenVisitorDoesNotExist()
    {
        var id = Guid.NewGuid();
        var args = new VisitorProfileArgs("2002-07-30", "Standard", "70");

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitorProfile?)null);

        Action act = () => _serviceService.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
        _repositoryMock.Verify(r => r.Update(It.IsAny<VisitorProfile>()), Times.Never);
    }
    #endregion
    #endregion
}
