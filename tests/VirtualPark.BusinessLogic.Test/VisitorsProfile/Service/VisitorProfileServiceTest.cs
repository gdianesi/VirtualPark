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
        var args = new VisitorProfileArgs("2002-07-30", "Standard", "85");

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitorProfile>(v =>
                v.DateOfBirth == args.DateOfBirth &&
                v.Membership == args.Membership)));

        var visitorProfile = _service.Create(args);

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

        _service.Remove(id);

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

        Action act = () => _service.Remove(id);

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

        var result = _service.Get(id);

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

        var act = () => _service.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAllVisitorProfiles_ShouldReturnList_WhenProfilesExist()
    {
        var vp1 = new VisitorProfile
        {
            DateOfBirth = new DateOnly(2000, 1, 1),
            Membership = Membership.Standard,
            Score = 80,
        };

        var vp2 = new VisitorProfile
        {
            DateOfBirth = new DateOnly(1998, 7, 30),
            Membership = Membership.Premium,
            Score = 95,
        };

        var expected = new List<VisitorProfile> { vp1, vp2 };

        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns(expected);

        var result = _service.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(vp1.Id);
        result[0].Membership.Should().Be(Membership.Standard);
        result[1].Id.Should().Be(vp2.Id);
        result[1].Membership.Should().Be(Membership.Premium);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAllVisitorProfiles_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetAll())
            .Returns((List<VisitorProfile>)null!);

        var act = _service.GetAll;

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Dont have any visitors profiles");

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

        _service.Update(args, id);

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

        Action act = () => _service.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
        _repositoryMock.Verify(r => r.Update(It.IsAny<VisitorProfile>()), Times.Never);
    }
    #endregion
    #endregion
}
