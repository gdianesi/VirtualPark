using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
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
    private Mock<IReadOnlyRepository<Ticket>> _ticketRepoMock = null!;
    private Mock<IClockAppService> _clockMock = null!;
    private VisitRegistrationService _service = null!;

    [TestInitialize]
    public void Initialize()
    {
        _visitorRepoMock = new Mock<IReadOnlyRepository<VisitorProfile>>(MockBehavior.Strict);
        _attractionRepoMock = new Mock<IReadOnlyRepository<Attraction>>(MockBehavior.Strict);
        _repositoryMock = new Mock<IRepository<VisitRegistration>>(MockBehavior.Strict);
        _ticketRepoMock = new Mock<IReadOnlyRepository<Ticket>>(MockBehavior.Strict);
        _clockMock = new Mock<IClockAppService>(MockBehavior.Strict);
        _service = new VisitRegistrationService(_repositoryMock.Object, _visitorRepoMock.Object, _attractionRepoMock.Object, _ticketRepoMock.Object, _clockMock.Object);
    }

    #region Create
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldCreateVisitRegistration_WhenVisitorAndAttractionsExist()
    {
        var fixedNow = new DateTime(2025, 10, 08, 14, 30, 00, DateTimeKind.Utc);
        _clockMock.Setup(c => c.Now()).Returns(fixedNow);

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var a1 = new Attraction { Name = "Roller" };
        var a2 = new Attraction { Name = "Wheel" };
        var a1Id = a1.Id;
        var a2Id = a2.Id;

        var ticket = new Ticket();
        var ticketId = ticket.Id;

        var args = new VisitRegistrationArgs(
            [a1Id.ToString(), a2Id.ToString()],
            visitorId.ToString(),
            ticketId.ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1Id))
            .Returns(a1);
        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a2Id))
            .Returns(a2);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        _repositoryMock
            .Setup(r => r.Add(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId &&
                vr.Visitor == visitor &&
                vr.TicketId == ticketId &&
                vr.Ticket == ticket &&
                vr.Attractions.Count == 2 &&
                vr.Attractions[0].Id == a1Id &&
                vr.Attractions[1].Id == a2Id &&
                vr.Date == fixedNow)));

        var result = _service.Create(args);

        result.Should().NotBeNull();
        result.VisitorId.Should().Be(visitorId);
        result.Visitor.Should().BeSameAs(visitor);
        result.TicketId.Should().Be(ticketId);
        result.Ticket.Should().BeSameAs(ticket);
        result.Attractions.Should().HaveCount(2);
        result.Attractions[0].Id.Should().Be(a1Id);
        result.Attractions[1].Id.Should().Be(a2Id);
        result.Date.Should().Be(fixedNow);

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
        _clockMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenVisitorDoesNotExist()
    {
        var visitorId = Guid.NewGuid();
        var args = new VisitRegistrationArgs([], visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns((VisitorProfile?)null);

        Action act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenAnyAttractionDoesNotExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var missingId = Guid.NewGuid();

        var args = new VisitRegistrationArgs(
            [missingId.ToString()],
            visitorId.ToString(), Guid.NewGuid().ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == missingId))
            .Returns((Attraction?)null);

        var act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _visitorRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Create_ShouldThrow_WhenTicketDoesNotExist()
    {
        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;

        var ticketId = Guid.NewGuid();
        var args = new VisitRegistrationArgs([], visitorId.ToString(), ticketId.ToString());

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns((Ticket?)null);

        Action act = () => _service.Create(args);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Ticket don't exist");

        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Get
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldReturnVisitRegistration_WhenExists()
    {
        var visit = new VisitRegistration();
        var id = visit.Id;

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;
        visit.VisitorId = visitorId;

        var ticket = new Ticket();
        var ticketId = ticket.Id;
        visit.TicketId = ticketId;

        var a1 = new Attraction { Name = "Placeholder" };
        visit.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns(a1);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(ticket);

        var result = _service.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Visitor.Should().BeSameAs(visitor);
        result.VisitorId.Should().Be(visitor.Id);
        result.Ticket.Should().BeSameAs(ticket);
        result.TicketId.Should().Be(ticket.Id);
        result.Attractions[0].Id.Should().Be(a1.Id);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenVisitRegistrationDoesNotExist()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Get_ShouldThrow_WhenAnyAttractionDoesNotExist()
    {
        var visit = new VisitRegistration();
        var id = visit.Id;

        var visitor = new VisitorProfile();
        var visitorId = visitor.Id;
        visit.VisitorId = visitorId;

        var ticket = new Ticket();
        var ticketId = ticket.Id;
        visit.TicketId = ticketId;

        var a1 = new Attraction { Name = "Placeholder" };
        visit.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns((Attraction?)null);

        var act = () => _service.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction don't exist");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Remove
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldDeleteVisitRegistration_WhenExists()
    {
        var vr = new VisitRegistration();
        var id = vr.Id;

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns(vr);

        _repositoryMock
            .Setup(r => r.Remove(vr));

        _service.Remove(id);

        _repositoryMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void Remove_ShouldThrow_WhenVisitRegistrationDoesNotExist()
    {
        var id = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == id))
            .Returns((VisitRegistration?)null);

        Action act = () => _service.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Visitor don't exist");

        _repositoryMock.VerifyAll();
    }
    #endregion
    #endregion

    #region Update
    [TestMethod]
    [TestCategory("Validation")]
    public void Update_ShouldApplyChanges_AndPersist_WhenVisitExists()
    {
        var visit = new VisitRegistration();
        var visitId = visit.Id;

        var oldVisitor = new VisitorProfile();
        var visitorId = oldVisitor.Id;
        visit.VisitorId = visitorId;

        var oldTicket = new Ticket();
        var ticketId = oldTicket.Id;
        visit.TicketId = ticketId;

        var a1 = new Attraction { Name = "Roller" };
        visit.Attractions = [a1];

        var newVisitorId = Guid.NewGuid();
        var newTicket = new Ticket();
        var newTicketId = newTicket.Id;

        var args = new VisitRegistrationArgs(
            [a1.Id.ToString()],
            newVisitorId.ToString(),
            newTicketId.ToString());

        _repositoryMock
            .Setup(r => r.Get(v => v.Id == visitId))
            .Returns(visit);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(oldVisitor);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == ticketId))
            .Returns(oldTicket);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == newTicketId))
            .Returns(newTicket);

        _visitorRepoMock
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(oldVisitor);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns(a1);

        _repositoryMock
            .Setup(r => r.Update(It.Is<VisitRegistration>(vr =>
                vr.Id == visitId &&
                vr.Ticket == newTicket &&
                vr.TicketId == newTicketId &&
                vr.Visitor == oldVisitor &&
                vr.VisitorId == newVisitorId &&
                vr.Attractions.Count == 1 &&
                vr.Attractions[0].Id == a1.Id)));

        _service.Update(args, visitId);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region GetAll
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldReturnVisitRegistrations_WithAllData()
    {
        var v1 = new VisitRegistration();
        var v2 = new VisitRegistration();

        var vp1 = new VisitorProfile();
        var vp1Id = vp1.Id;
        v1.VisitorId = vp1Id;
        var vp2 = new VisitorProfile();
        var vp2Id = vp2.Id;
        v2.VisitorId = vp2Id;

        var t1 = new Ticket();
        var t1Id = t1.Id;
        v1.TicketId = t1Id;
        var t2 = new Ticket();
        var t2Id = t2.Id;
        v2.TicketId = t2Id;

        var a1 = new Attraction { Name = "A1" };
        var a1Id = a1.Id;

        v1.Attractions = [a1];

        _repositoryMock
            .Setup(r => r.GetAll(null))
            .Returns([v1, v2]);

        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == t1Id))
            .Returns(t1);
        _ticketRepoMock
            .Setup(r => r.Get(t => t.Id == t2Id))
            .Returns(t2);

        _visitorRepoMock
            .Setup(r => r.Get(vp => vp.Id == vp1Id))
            .Returns(vp1);
        _visitorRepoMock
            .Setup(r => r.Get(vp => vp.Id == vp2Id))
            .Returns(vp2);

        _attractionRepoMock
            .Setup(r => r.Get(x => x.Id == a1.Id))
            .Returns(a1);

        var result = _service.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var r1 = result[0];
        r1.Visitor.Should().BeSameAs(vp1);
        r1.Ticket.Should().BeSameAs(t1);
        r1.Attractions.Should().HaveCount(1);
        r1.Attractions[0].Id.Should().Be(a1Id);

        var r2 = result[1];
        r2.Visitor.Should().BeSameAs(vp2);
        r2.Ticket.Should().BeSameAs(t2);

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_ShouldThrow_WhenRepositoryReturnsNull()
    {
        _repositoryMock
            .Setup(r => r.GetAll(null))
            .Returns((List<VisitRegistration>)null!);

        var act = _service.GetAll;

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Dont have any visit registrations");

        _repositoryMock.VerifyAll();
        _visitorRepoMock.VerifyAll();
        _ticketRepoMock.VerifyAll();
        _attractionRepoMock.VerifyAll();
    }
    #endregion
    #endregion
}
