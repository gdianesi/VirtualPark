using System.Linq.Expressions;
using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Attractions.Models;
using VirtualPark.BusinessLogic.Attractions.Services;
using VirtualPark.BusinessLogic.ClocksApp.Service;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Incidences.Entity;
using VirtualPark.BusinessLogic.Incidences.Service;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Attractions.Services;

[TestClass]
[TestCategory("Attractions")]
[TestCategory("AttractionService")]
public class AttractionServiceTest
{
    private AttractionService _attractionService = null!;
    private Mock<IRepository<Attraction>> _mockAttractionRepository = null!;
    private Mock<IRepository<Ticket>> _mockTicketRepository = null!;
    private Mock<IRepository<VisitorProfile>> _mockVisitorProfileRepository = null!;
    private Mock<IRepository<VisitRegistration>> _mockVisitorRegistrationRepository = null!;
    private Mock<IRepository<Event>> _mockEventRepository = null!;
    private AttractionArgs _attractionArgs = null!;
    private Mock<IClockAppService> _mockClock = null!;
    private Mock<IReadOnlyRepository<Attraction>> _mockReadOnlyAttractionRepository = null!;
    private Mock<IRepository<Incidence>> _mockIncidenceRepository = null!;
    private Mock<IIncidenceService> _mockIncidenceService = null!;
    private readonly DateTime _now = new DateTime(2025, 10, 15, 10, 0, 0);

    [TestInitialize]
    public void Initialize()
    {
        _mockAttractionRepository = new Mock<IRepository<Attraction>>(MockBehavior.Strict);
        _mockVisitorProfileRepository = new Mock<IRepository<VisitorProfile>>(MockBehavior.Strict);
        _mockTicketRepository = new Mock<IRepository<Ticket>>(MockBehavior.Strict);
        _mockVisitorRegistrationRepository = new Mock<IRepository<VisitRegistration>>(MockBehavior.Strict);
        _mockEventRepository = new Mock<IRepository<Event>>(MockBehavior.Strict);
        _mockClock = new Mock<IClockAppService>(MockBehavior.Strict);
        _mockIncidenceService = new Mock<IIncidenceService>(MockBehavior.Strict);
        _mockIncidenceRepository = new Mock<IRepository<Incidence>>(MockBehavior.Strict);

        _mockClock.Setup(c => c.Now()).Returns(_now);

        _attractionService = new AttractionService(_mockAttractionRepository.Object,
            _mockVisitorProfileRepository.Object, _mockTicketRepository.Object, _mockEventRepository.Object,
            _mockVisitorRegistrationRepository.Object, _mockClock.Object, _mockIncidenceService.Object);

        _attractionArgs = new AttractionArgs("RollerCoaster", "The Big Bang", "13", "500", "Description", "50", "true");
        _mockReadOnlyAttractionRepository = new Mock<IReadOnlyRepository<Attraction>>(MockBehavior.Strict);
    }

    #region Create

    [TestMethod]
    public void Create_WhenArgsAreValid_ShouldReturnIdAndPersistEntity()
    {
        var args = _attractionArgs;

        _mockReadOnlyAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        Attraction? added = null;
        _mockAttractionRepository
            .Setup(r => r.Add(It.IsAny<Attraction>()))
            .Callback<Attraction>(a => added = a);

        Guid id = _attractionService.Create(args);

        id.Should().NotBeEmpty();
        added.Should().NotBeNull();
        added!.Id.Should().Be(id);

        added.Name.Should().Be(args.Name);
        added.Type.Should().Be(args.Type);
        added.MiniumAge.Should().Be(args.MiniumAge);
        added.Capacity.Should().Be(args.Capacity);
        added.Description.Should().Be(args.Description);
        added.CurrentVisitors.Should().Be(args.CurrentVisitor);
        added.Available.Should().Be(args.Available);

        _mockAttractionRepository.Verify(
            r => r.Add(It.Is<Attraction>(a => a.Id == id)),
            Times.Once);
    }
    #endregion

    #region ValidationName
    [TestCategory("Validation")]
    [TestMethod]
    public void Create_WhenNameIsEmpty_ShouldThrowException()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(a => a.Name.Equals(string.Empty, StringComparison.CurrentCultureIgnoreCase)))
            .Returns(true);

        Assert.ThrowsException<ArgumentException>(
            () => _attractionService.ValidateAttractionName(string.Empty));
    }

    [TestMethod]
    public void Create_WhenNameAlreadyExists_ShouldThrowDuplicateException()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(a => a.Name.Equals(string.Empty, StringComparison.CurrentCultureIgnoreCase)))
            .Returns(true);

        Action act = () => _attractionService.Create(_attractionArgs);

        act.Should().Throw<Exception>();

        _mockAttractionRepository.Verify(r => r.Add(It.IsAny<Attraction>()), Times.Never);
    }

    [TestMethod]
    public void Create_WhenNameIsValid_ShouldCreate()
    {
        _mockAttractionRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Add(It.IsAny<Attraction>()));

        Action act = () => _attractionService.Create(_attractionArgs);

        act.Should().NotThrow();
        _mockAttractionRepository.Verify(r => r.Add(It.IsAny<Attraction>()), Times.Once);
    }

    [TestMethod]
    public void ValidateAttractionName_WhenNameAlreadyExists_ShouldThrowException()
    {
        var name = "RollerCoaster";

        _mockAttractionRepository
            .Setup(r => r.Exist(a => a.Name.ToLower() == name.ToLower()))
            .Returns(true);

        Action act = () => _attractionService.ValidateAttractionName(name);

        act.Should().Throw<Exception>()
            .WithMessage("Attraction name already exists.");

        _mockAttractionRepository.VerifyAll();
    }
    #endregion

    #region MapToEntity

    [TestMethod]
    [TestCategory("Validation")]
    public void MapToEntity_WhenArgsAreValid_ShouldReturnAttractionEntity()
    {
        var name = _attractionArgs.Name;

        _mockAttractionRepository
            .Setup(r => r.Exist(a => a.Name.ToLower() == name.ToLower()))
            .Returns(false);

        var attraction = _attractionService.MapToEntity(_attractionArgs);

        attraction.Should().NotBeNull();
        attraction.Should().BeEquivalentTo(_attractionArgs, opt => opt.ExcludingMissingMembers());

        _mockAttractionRepository.VerifyAll();

        _mockAttractionRepository.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenSomeAttractionsAreDeleted_ShouldReturnOnlyNotDeleted()
    {
        var a1 = new Attraction { Id = Guid.NewGuid(), Name = "RollerCoaster", IsDeleted = false };
        var a2 = new Attraction { Id = Guid.NewGuid(), Name = "BoatRide", IsDeleted = true };
        var a3 = new Attraction { Id = Guid.NewGuid(), Name = "FreeFall", IsDeleted = false };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Expression<Func<Attraction, bool>> filter) => new List<Attraction> { a1, a2, a3 }
                .Where(filter.Compile())
                .ToList());

        var result = _attractionService.GetAll();

        result.Should().HaveCount(2, "only attractions where IsDeleted == false must be returned");
        result.Should().Contain(a1);
        result.Should().Contain(a3);
        result.Should().NotContain(a2, "deleted items must not be returned");

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenAttractionsExist_ShouldReturnAllAttractions()
    {
        var attractions = new List<Attraction>
        {
            new Attraction { Name = "RollerCoaster", Type = AttractionType.RollerCoaster, Capacity = 50 },
            new Attraction { Name = "FerrisWheel",  Type = AttractionType.Simulator,  Capacity = 100 }
        };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns(attractions);

        var result = _attractionService.GetAll();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        result.Should().Contain(a => a.Name == "RollerCoaster");
        result.Should().Contain(a => a.Name == "FerrisWheel");

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void GetAll_WhenNoAttractionsExist_ShouldReturnEmptyList()
    {
        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns([]);

        var result = _attractionService.GetAll();

        result.Should().NotBeNull();
        result.Should().BeEmpty();

        _mockAttractionRepository.Verify(
            r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()),
            Times.Once);
    }
    #endregion

    #region Get
    [TestMethod]
    public void Get_WhenAttractionExists_ShouldReturnAttraction()
    {
        var id = Guid.NewGuid();
        var expected = new Attraction { Id = id, Name = "RollerCoaster", Capacity = 50 };

        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == id)).Returns(expected);

        var result = _attractionService.Get(id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(id);
        result.Name.Should().Be("RollerCoaster");
    }
    #endregion

    #region Update

    [TestMethod]
    [TestCategory("Validation")]
    public void ApplyArgsToEntity_ShouldCopyAllPropertiesFromArgs()
    {
        var entity = new Attraction();

        AttractionService.ApplyArgsToEntity(entity, _attractionArgs);

        entity.Name.Should().Be(_attractionArgs.Name);
        entity.Type.Should().Be(_attractionArgs.Type);
        entity.MiniumAge.Should().Be(_attractionArgs.MiniumAge);
        entity.Capacity.Should().Be(_attractionArgs.Capacity);
        entity.Description.Should().Be(_attractionArgs.Description);
        entity.CurrentVisitors.Should().Be(_attractionArgs.CurrentVisitor);
        entity.Available.Should().Be(_attractionArgs.Available);
    }
    #endregion

    #region Update
    [TestMethod]
    [TestCategory("Validations")]
    public void Update_ShouldCopyAllPropertiesFromArgs_AndPersist()
    {
        var id = Guid.NewGuid();

        var existing = new Attraction
        {
            Id = id,
            Name = "Old name",
            Type = AttractionType.RollerCoaster,
            MiniumAge = 10,
            Capacity = 100,
            Description = "Old desc",
            CurrentVisitors = 0,
            Available = false
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(existing);

        Attraction? updated = null;
        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()))
            .Callback<Attraction>(a => updated = a);

        var args = new AttractionArgs(
            type: "Simulator",
            name: "Example Attraction",
            miniumAge: "13",
            capacity: "500",
            description: "New description",
            currentVisitor: "50",
            available: "true");

        _attractionService.Update(args, id);

        updated.Should().NotBeNull();
        updated!.Id.Should().Be(id);
        updated.Name.Should().Be("Example Attraction");
        updated.Type.Should().Be(AttractionType.Simulator);
        updated.MiniumAge.Should().Be(13);
        updated.Capacity.Should().Be(500);
        updated.Description.Should().Be("New description");
        updated.CurrentVisitors.Should().Be(50);
        updated.Available.Should().BeTrue();

        _mockAttractionRepository.Verify(r => r.Update(It.IsAny<Attraction>()), Times.Once);
        _mockAttractionRepository.VerifyAll();
    }
    #endregion

    #region Remove

    [TestMethod]
    public void Remove_WhenAttractionExists_ShouldSoftDeleteAndUpdate()
    {
        var id = Guid.NewGuid();
        var existing = new Attraction { Id = id, Name = "To Remove" };

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(It.IsAny<Guid>(), It.IsAny<DateTime>()))
            .Returns(false);

        _mockEventRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns([]);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(existing);

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        _attractionService.Remove(id);

        existing.IsDeleted.Should().BeTrue();

        _mockAttractionRepository.Verify(r => r.Update(
            It.Is<Attraction>(a => a.Id == id && a.IsDeleted == true)), Times.Once);

        _mockAttractionRepository.Verify(r => r.Remove(It.IsAny<Attraction>()), Times.Never);
    }

    [TestMethod]
    public void Remove_WhenAttractionHasFutureEvent_ShouldThrow()
    {
        var id = Guid.NewGuid();
        var attraction = new Attraction { Id = id };
        var ev = new Event
        {
            Date = _now.AddDays(5),
            Attractions = [attraction]
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(attraction);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>()))
            .Returns(false);

        _mockEventRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns([ev]);

        Action act = () => _attractionService.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage("Attraction cannot be deleted because it is associated with a future event.");
    }

    [TestMethod]
    public void Remove_WhenAttractionHasPastEvents_ShouldSoftDeleteAttraction()
    {
        var id = Guid.NewGuid();

        var attraction = new Attraction { Id = id, Name = "Roller X" };

        var pastEvent = new Event
        {
            Id = Guid.NewGuid(),
            Date = _now.AddDays(-10),
            Attractions = [attraction]
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(attraction);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(
                It.IsAny<Guid>(),
                It.IsAny<DateTime>()))
            .Returns(false);

        _mockEventRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns([pastEvent]);

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        _attractionService.Remove(id);

        attraction.IsDeleted.Should().BeTrue();
    }

    [TestMethod]
    public void Remove_WhenAttractionHasActiveIncidence_ShouldThrow()
    {
        var id = Guid.NewGuid();

        var attraction = new Attraction { Id = id };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(attraction);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(id, _now))
            .Returns(true);

        Action act = () => _attractionService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Attraction cannot be deleted because it has active incidences.");
    }

    [TestMethod]
    public void Remove_WhenNoDependencies_ShouldSoftDeleteAttraction()
    {
        var id = Guid.NewGuid();
        var attraction = new Attraction { Id = id };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns(attraction);

        _mockEventRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Event, bool>>>()))
            .Returns([]);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(id, _now))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Update(attraction));

        _attractionService.Remove(id);

        attraction.IsDeleted.Should().BeTrue();
    }

    [TestMethod]
    public void Remove_WhenAttractionDoesNotExist_ShouldThrow_AndNotCallRemove()
    {
        var id = Guid.NewGuid();

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == id))
            .Returns((Attraction?)null);

        Action act = () => _attractionService.Remove(id);

        act.Should()
            .Throw<InvalidOperationException>()
            .WithMessage($"Attraction with id {id} not found.");

        _mockAttractionRepository.Verify(r => r.Remove(It.IsAny<Attraction>()), Times.Never);
        _mockAttractionRepository.VerifyAll();
    }

    #endregion

    #region ValidateEntryByNfc

    #region Failure

    [TestMethod]
    public void ValidateEntryByNfc_WhenAttractionIsNotAvailable_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Available = false };
        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2002, 02, 01) };

        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorProfileRepository.Setup(r => r.Get(a => a.Id == visitorId)).Returns(visitor);

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().Be(false);
    }

    [TestMethod]
    public void ValidateEntryByNfc_WhenVisitorIsTooYoung_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, MiniumAge = 18, Available = true };
        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2018, 03, 20) };

        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorProfileRepository.Setup(r => r.Get(v => v.Id == visitorId)).Returns(visitor);

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().Be(false);
    }

    [TestMethod]
    public void ValidateEntryByNfc_WhenAttractionIsAtCapacity_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Capacity = 5,
            CurrentVisitors = 5,
            Available = true,
            MiniumAge = 10
        };
        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2004, 10, 06) };

        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorProfileRepository.Setup(r => r.Get(v => v.Id == visitorId)).Returns(visitor);
        _mockAttractionRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>())).Returns(attraction);
        _mockVisitorProfileRepository.Setup(r => r.Get(It.IsAny<Expression<Func<VisitorProfile, bool>>>()))
            .Returns(visitor);

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().Be(false);
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateEntryByNfc_WhenVisitorAlreadyInActiveVisit_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Capacity = 10,
            CurrentVisitors = 3,
            MiniumAge = 10,
            Available = true
        };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2000, 1, 1) };

        var activeVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = DateTime.Today,
            IsActive = true
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorProfileRepository
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns(activeVisit);

        _mockAttractionRepository
            .Setup(r => r.Update(attraction));

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Maintenance")]
    public void ValidateEntryByNfc_WhenAttractionHasActiveIncidence_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Available = true,
            Capacity = 10,
            CurrentVisitors = 2,
            MiniumAge = 10
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorProfileRepository
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, _now))
            .Returns(true);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns((VisitRegistration?)null);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Add(It.IsAny<VisitRegistration>()));

        _mockVisitorRegistrationRepository
            .Setup(r => r.Update(It.IsAny<VisitRegistration>()));

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().BeFalse("because the attraction is under active maintenance/incidence");

        _mockIncidenceService.VerifyAll();
    }

    #endregion

    #region Success
    [TestMethod]
    public void ValidateEntryByNfc_WhenVisitorMeetsAllRequirements_ShouldReturnTrueAndIncrementCurrentVisitors()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Name = "Montaña Rusa",
            Capacity = 10,
            CurrentVisitors = 2,
            MiniumAge = 10,
            Available = true
        };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2000, 1, 1) };

        var inactiveVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = DateTime.Today,
            IsActive = false
        };

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorProfileRepository
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns(inactiveVisit);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Update(It.IsAny<VisitRegistration>()));

        _mockAttractionRepository
            .Setup(r => r.Update(attraction));

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("Maintenance")]
    public void ValidateEntryByNfc_WhenAttractionHasNoIncidence_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Available = true,
            Capacity = 10,
            CurrentVisitors = 2,
            MiniumAge = 10
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };

        var inactiveVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = new DateTime(2025, 10, 15),
            IsActive = false
        };

        _mockClock
            .Setup(c => c.Now())
            .Returns(new DateTime(2025, 10, 15));

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorProfileRepository
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);
        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns(inactiveVisit);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Update(It.IsAny<VisitRegistration>()));

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        var result = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        result.Should().BeTrue();
    }

    #endregion
    #endregion

    #region ValidateEntryByQr

    #region Failure

    [TestMethod]
    public void ValidateEntryByQr_WhenTicketDoesNotExist_ShouldReturnFalse()
    {
        _mockTicketRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns((Ticket?)null);
        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Attraction)null!);

        var result = _attractionService.ValidateEntryByQr(Guid.NewGuid(), Guid.NewGuid());

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ValidateEntryByQr_WhenTicketIsExpired_ShouldReturnFalse()
    {
        var qrId = Guid.NewGuid();
        var ticket = new Ticket { QrId = qrId, Date = DateTime.Today.AddDays(-1), Type = EntranceType.General };

        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Attraction)null!);
        _mockTicketRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(ticket);

        var result = _attractionService.ValidateEntryByQr(Guid.NewGuid(), qrId);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ValidateEntryByQr_WhenAttractionIsFull_ShouldReturnFalse()
    {
        var qrId = Guid.NewGuid();
        var ticket = new Ticket { QrId = qrId, Date = DateTime.Today, Type = EntranceType.General };

        var attraction = new Attraction { Id = Guid.NewGuid(), Capacity = 2, CurrentVisitors = 2, Available = true };

        _mockTicketRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>())).Returns(attraction);

        var result = _attractionService.ValidateEntryByQr(attraction.Id, qrId);

        result.Should().BeFalse();
    }

    [TestMethod]
    public void ValidateEntryByQr_WhenEventTicketButEventDoesNotExist_ShouldReturnFalse()
    {
        var qrId = Guid.NewGuid();
        var ticket = new Ticket
        {
            QrId = qrId,
            Date = DateTime.Today,
            Type = EntranceType.Event,
            EventId = Guid.NewGuid()
        };
        _mockAttractionRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns((Attraction)null!);

        _mockTicketRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Ticket, bool>>>())).Returns(ticket);
        _mockEventRepository.Setup(r => r.Get(It.IsAny<Expression<Func<Event, bool>>>())).Returns((Event?)null);

        var result = _attractionService.ValidateEntryByQr(Guid.NewGuid(), qrId);

        result.Should().BeFalse();
    }
    #endregion
    [TestMethod]
    public void ValidateEntryByQr_WhenSpecialEventOutsideTimeWindow_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Capacity = 10, Available = true };

        var ev = new Event
        {
            Id = eventId,
            Date = DateTime.Now,
            Capacity = 10,
            Attractions = [attraction]
        };

        var visitor = new VisitorProfile
        {
            Id = Guid.NewGuid(),
            DateOfBirth = new DateOnly(2000, 01, 01)
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            QrId = qrId,
            Date = DateTime.Now.AddHours(-5),
            Type = EntranceType.Event,
            EventId = eventId,
            Visitor = visitor
        };

        var visitorId = visitor.Id;

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockTicketRepository
            .Setup(r => r.Get(t => t.QrId == qrId))
            .Returns(ticket);

        _mockEventRepository
            .Setup(r => r.Get(e => e.Id == ticket.EventId))
            .Returns(ev);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns((VisitRegistration?)null);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Add(It.Is<VisitRegistration>(v => v.VisitorId == visitorId)));

        var result = _attractionService.ValidateEntryByQr(attractionId, qrId);

        result.Should().BeFalse();
    }

    [TestMethod]
    [TestCategory("Maintenance")]
    public void ValidateEntryByQr_WhenAttractionHasActiveIncidence_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            QrId = qrId,
            Date = _now,
            Type = EntranceType.General,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var attraction = new Attraction
        {
            Id = attractionId,
            Available = true,
            Capacity = 10,
            CurrentVisitors = 2
        };

        _mockTicketRepository
            .Setup(r => r.Get(t => t.QrId == qrId))
            .Returns(ticket);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<VisitRegistration, bool>>>()))
            .Returns((VisitRegistration?)null);

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, _now))
            .Returns(true);

        var result = _attractionService.ValidateEntryByQr(attractionId, qrId);

        result.Should().BeFalse("because the attraction has an active incidence preventing entry");

        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockIncidenceService.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateEntryByQr_WhenVisitorAlreadyInActiveVisit_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var qrId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            QrId = qrId,
            Date = DateTime.Today,
            Type = EntranceType.General,
            Visitor = visitor
        };

        var attraction = new Attraction
        {
            Id = attractionId,
            Capacity = 10,
            CurrentVisitors = 3,
            MiniumAge = 10,
            Available = true
        };

        var activeVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = DateTime.Today,
            IsActive = true,
            Attractions = [attraction],
            Ticket = ticket,
            TicketId = ticket.Id
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockTicketRepository
            .Setup(r => r.Get(t => t.QrId == qrId))
            .Returns(ticket);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns(activeVisit);

        var result = _attractionService.ValidateEntryByQr(attractionId, qrId);

        result.Should().BeFalse("because the visitor already has an active visit");
        _mockVisitorRegistrationRepository.Verify(r => r.Update(It.IsAny<VisitRegistration>()), Times.Never);
        _mockVisitorRegistrationRepository.Verify(r => r.Add(It.IsAny<VisitRegistration>()), Times.Never);
    }

    [TestMethod]
    public void ValidateEntryByQr_Event_AttractionInEvent_WithinWindow_AndCapacityAvailable_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Capacity = 5, CurrentVisitors = 2, MiniumAge = 0, Available = true };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Show",
            Date = _now,
            Capacity = 10,
            Cost = 0,
            Attractions = [attraction]
        };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2000, 1, 1) };

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = _now,
            Type = EntranceType.Event,
            Event = ev,
            EventId = ev.Id,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var existingVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = _now.Date,
            IsActive = false,
            Attractions = []
        };
        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        _mockTicketRepository.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorRegistrationRepository.Setup(r => r.Get(v => v.VisitorId == visitorId)).Returns(existingVisit);
        _mockVisitorRegistrationRepository.Setup(r => r.Update(existingVisit));
        _mockEventRepository.Setup(r => r.Get(e => e.Id == ticket.EventId)).Returns(ev);
        _mockTicketRepository.Setup(r => r.GetAll(t => t.EventId == ticket.EventId)).Returns([]);
        _mockAttractionRepository.Setup(r => r.Update(attraction));

        var ok = _attractionService.ValidateEntryByQr(attractionId, qrId);

        ok.Should().BeTrue();

        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
        _mockEventRepository.VerifyAll();
        _mockClock.VerifyAll();
    }

    [TestMethod]
    public void ValidateEntryByQr_Event_WhenCapacityExceeded_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Capacity = 5, CurrentVisitors = 2, MiniumAge = 0, Available = true };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Show",
            Date = _now,
            Capacity = 1,
            Cost = 0,
            Attractions = [attraction]
        };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2000, 1, 1) };

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = _now,
            Type = EntranceType.Event,
            Event = ev,
            EventId = ev.Id,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var existingVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = _now.Date,
            IsActive = false,
            Attractions = []
        };
        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        _mockTicketRepository.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorRegistrationRepository.Setup(r => r.Get(v => v.VisitorId == visitorId)).Returns(existingVisit);
        _mockVisitorRegistrationRepository.Setup(r => r.Update(existingVisit));
        _mockEventRepository.Setup(r => r.Get(e => e.Id == ticket.EventId)).Returns(ev);
        _mockTicketRepository.Setup(r => r.GetAll(t => t.EventId == ticket.EventId)).Returns([ticket]);

        var ok = _attractionService.ValidateEntryByQr(attractionId, qrId);

        ok.Should().BeFalse();

        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
        _mockEventRepository.VerifyAll();
        _mockClock.VerifyAll();
    }

    [TestMethod]
    public void ValidateEntryByQr_Event_WhenAttractionNotInEvent_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Capacity = 5, CurrentVisitors = 0, MiniumAge = 0, Available = true };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Show",
            Date = _now,
            Capacity = 10,
            Cost = 0,
            Attractions = []
        };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2000, 1, 1) };

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = _now,
            Type = EntranceType.Event,
            Event = ev,
            EventId = ev.Id,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var existingVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = _now.Date,
            IsActive = false,
            Attractions = []
        };
        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);
        _mockTicketRepository.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorRegistrationRepository.Setup(r => r.Get(v => v.VisitorId == visitorId)).Returns(existingVisit);
        _mockVisitorRegistrationRepository.Setup(r => r.Update(existingVisit));
        _mockEventRepository.Setup(r => r.Get(e => e.Id == ticket.EventId)).Returns(ev);

        var ok = _attractionService.ValidateEntryByQr(attractionId, qrId);

        ok.Should().BeFalse();

        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
        _mockEventRepository.VerifyAll();
        _mockClock.VerifyAll();
    }

    [TestMethod]
    public void ValidateEntryByQr_General_WhenExistingInactiveVisit_ShouldActivateAndRegister_ReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction { Id = attractionId, Capacity = 5, CurrentVisitors = 2, MiniumAge = 0, Available = true };

        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = new DateOnly(2002, 2, 15) };

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = _now,
            Type = EntranceType.General,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var existingVisit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = _now.Date,
            IsActive = false,
            Attractions = []
        };
        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        _mockVisitorRegistrationRepository.Setup(r => r.Get(v => v.VisitorId == visitorId)).Returns(existingVisit);
        _mockVisitorRegistrationRepository.Setup(r => r.Update(existingVisit));
        _mockTicketRepository.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockAttractionRepository.Setup(r => r.Update(attraction));

        var ok = _attractionService.ValidateEntryByQr(attractionId, qrId);

        ok.Should().BeTrue();
        attraction.CurrentVisitors.Should().Be(3);

        _mockVisitorRegistrationRepository.VerifyAll();
        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockClock.VerifyAll();
    }
    #endregion

    #region Success
    [TestMethod]
    [TestCategory("Maintenance")]
    public void ValidateEntryByQr_WhenAttractionHasNoIncidence_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Available = true,
            Capacity = 10,
            CurrentVisitors = 2,
            MiniumAge = 0
        };

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2000, 1, 1)
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            QrId = qrId,
            Date = _now,
            Type = EntranceType.General,
            Visitor = visitor,
            VisitorProfileId = visitorId
        };

        var visit = new VisitRegistration
        {
            VisitorId = visitorId,
            Visitor = visitor,
            Date = _now.Date,
            IsActive = false,
            Attractions = []
        };

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockTicketRepository
            .Setup(r => r.Get(t => t.QrId == qrId))
            .Returns(ticket);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns(visit);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Update(It.IsAny<VisitRegistration>()));

        _mockAttractionRepository
            .Setup(r => r.Update(It.IsAny<Attraction>()));

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, _now))
            .Returns(false);

        _mockClock
            .Setup(c => c.Now())
            .Returns(_now);

        var ok = _attractionService.ValidateEntryByQr(attractionId, qrId);

        ok.Should().BeTrue();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateEntryByQr_WhenTicketIsGeneralAndValid_ShouldReturnTrueAndCreateVisitRegistration()
    {
        var attractionId = Guid.NewGuid();
        var qrId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();

        var visitor = new VisitorProfile
        {
            Id = visitorId,
            DateOfBirth = new DateOnly(2002, 02, 15)
        };

        var ticket = new Ticket
        {
            QrId = qrId,
            Date = _now,
            Type = EntranceType.General,
            Visitor = visitor
        };

        var attraction = new Attraction
        {
            Id = attractionId,
            Capacity = 5,
            CurrentVisitors = 2,
            MiniumAge = 0,
            Available = true
        };
        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);
        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns((VisitRegistration?)null);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Add(It.Is<VisitRegistration>(v =>
                    (v.VisitorId == visitorId &&
                    v.IsActive == true &&
                    v.Date == _now.Date &&
                    v.TicketId == ticket.Id) || v.TicketId == Guid.Empty)));

        _mockTicketRepository.Setup(r => r.Get(t => t.QrId == qrId)).Returns(ticket);
        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockAttractionRepository.Setup(r => r.Update(attraction));

        var result = _attractionService.ValidateEntryByQr(attractionId, qrId);

        result.Should().BeTrue();
        attraction.CurrentVisitors.Should().Be(3);

        _mockVisitorRegistrationRepository.VerifyAll();
        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockClock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateEntryByNfc_WhenVisitorIsExactlyMinAgeToday_ShouldReturnTrue()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var minAge = 12;

        var attraction = new Attraction
        {
            Id = attractionId,
            MiniumAge = minAge,
            Available = true,
            Capacity = 100,
            CurrentVisitors = 0
        };

        var dob = DateOnly.FromDateTime(_now.Date.AddYears(-minAge));
        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = dob };

        _mockIncidenceService
            .Setup(s => s.HasActiveIncidenceForAttraction(attractionId, It.IsAny<DateTime>()))
            .Returns(false);

        _mockAttractionRepository
            .Setup(r => r.Get(a => a.Id == attractionId))
            .Returns(attraction);

        _mockVisitorProfileRepository
            .Setup(r => r.Get(v => v.Id == visitorId))
            .Returns(visitor);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Get(v => v.VisitorId == visitorId))
            .Returns((VisitRegistration?)null);

        _mockVisitorRegistrationRepository
            .Setup(r => r.Add(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId &&
                vr.IsActive == false)));

        _mockAttractionRepository
            .Setup(r => r.Update(It.Is<Attraction>(a => a.Id == attractionId)));

        _mockVisitorRegistrationRepository
            .Setup(r => r.Update(It.Is<VisitRegistration>(vr =>
                vr.VisitorId == visitorId && vr.IsActive == true)));

        var ok = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        ok.Should().BeTrue();
        _mockAttractionRepository.VerifyAll();
        _mockVisitorProfileRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void ValidateEntryByNfc_WhenVisitorIsOneDayYoungerThanMinAge_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var visitorId = Guid.NewGuid();
        var minAge = 12;

        var attraction = new Attraction
        {
            Id = attractionId,
            MiniumAge = minAge,
            Available = true,
            Capacity = 100,
            CurrentVisitors = 0
        };

        var dob = DateOnly.FromDateTime(_now.Date.AddYears(-minAge).AddDays(1));
        var visitor = new VisitorProfile { Id = visitorId, DateOfBirth = dob };

        _mockAttractionRepository.Setup(r => r.Get(a => a.Id == attractionId)).Returns(attraction);
        _mockVisitorProfileRepository.Setup(r => r.Get(v => v.Id == visitorId)).Returns(visitor);

        var ok = _attractionService.ValidateEntryByNfc(attractionId, visitorId);

        ok.Should().BeFalse();

        _mockAttractionRepository.VerifyAll();
        _mockVisitorProfileRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
        _mockClock.VerifyAll();
    }
    #endregion

    #region ValidateEntry
    [TestMethod]
    public void ValidateEventEntry_WhenEventIsNull_ShouldReturnFalse()
    {
        var attractionId = Guid.NewGuid();
        var eventId = Guid.NewGuid();

        var attraction = new Attraction
        {
            Id = attractionId,
            Available = true,
            Capacity = 10,
            CurrentVisitors = 0
        };

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            Date = DateTime.Now,
            Type = EntranceType.Event
        };

        _mockEventRepository
            .Setup(r => r.Get(e => e.Id == ticket.EventId))
            .Returns((Event?)null);

        var result = _attractionService.ValidateEventEntry(ticket, attraction);

        result.Should().BeFalse("porque el evento no existe");

        _mockEventRepository.Verify(
            r => r.Get(e => e.Id == ticket.EventId),
            Times.Once);
    }
    #endregion

    #region Report
    [TestMethod]
    public void AttractionsReport_ShouldThrow_WhenFromGreaterThanTo()
    {
        var from = new DateTime(2025, 10, 10);
        var to = new DateTime(2025, 10, 1);

        Action act = () => _attractionService.AttractionsReport(from, to);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("From date must be less than or equal to To date.");
    }

    [TestMethod]
    public void AttractionsReport_ShouldReturnZeroForEachAttraction_WhenThereAreNoVisits()
    {
        var from = new DateTime(2025, 10, 1);
        var to = new DateTime(2025, 10, 31);

        var a1 = new Attraction { Name = "Montaña Rusa" };
        var a2 = new Attraction { Name = "Simulador B" };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns([a1, a2]);

        _mockVisitorRegistrationRepository
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<VisitRegistration, bool>>>(),
                It.IsAny<Func<IQueryable<VisitRegistration>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<VisitRegistration, object>>>()))
            .Returns([]);

        var result = _attractionService.AttractionsReport(from, to);

        result.Should().HaveCount(2);
        result[0].Should().Be($"{a1.Name}\t0");
        result[1].Should().Be($"{a2.Name}\t0");

        _mockAttractionRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
    }

    [TestMethod]
    public void AttractionsReport_ShouldCountOnlyVisitsInsideRange()
    {
        var from = new DateTime(2025, 10, 10);
        var to = new DateTime(2025, 10, 20);

        var a1 = new Attraction { Id = Guid.NewGuid(), Name = "Montaña Rusa" };
        var a2 = new Attraction { Id = Guid.NewGuid(), Name = "Simulador B" };

        _mockAttractionRepository
            .Setup(r => r.GetAll(It.IsAny<Expression<Func<Attraction, bool>>>()))
            .Returns([a1, a2]);

        var visitInside = new VisitRegistration
        {
            Date = new DateTime(2025, 10, 15),
            Attractions = [a1]
        };
        var visitOutside = new VisitRegistration
        {
            Date = new DateTime(2025, 10, 25),
            Attractions = [a1, a2]
        };

        _mockVisitorRegistrationRepository
            .Setup(r => r.GetAll(
                It.IsAny<Expression<Func<VisitRegistration, bool>>>(),
                It.IsAny<Func<IQueryable<VisitRegistration>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<VisitRegistration, object>>>()))
            .Returns([visitInside, visitOutside]);

        var result = _attractionService.AttractionsReport(from, to);

        result.Should().Contain($"{a1.Name}\t1");
        result.Should().Contain($"{a2.Name}\t0");

        _mockAttractionRepository.VerifyAll();
        _mockVisitorRegistrationRepository.VerifyAll();
    }
    #endregion

    #region ValidateEventEntry
    [TestMethod]
    public void ValidateEventEntry_WhenAttractionNotInEvent_ShouldReturnFalse()
    {
        var attraction = new Attraction { Id = Guid.NewGuid(), Available = true, Capacity = 50, CurrentVisitors = 0 };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Show",
            Date = _now,
            Capacity = 100,
            Cost = 0,
            Attractions = []
        };

        var ticket = new Ticket
        {
            Date = _now,
            Type = EntranceType.Event,
            Event = ev,
            EventId = ev.Id,
            Visitor = new VisitorProfile { Id = Guid.NewGuid() },
            VisitorProfileId = Guid.NewGuid()
        };

        _mockEventRepository
            .Setup(r => r.Get(e => e.Id == ticket.EventId))
            .Returns(ev);

        var ok = _attractionService.ValidateEventEntry(ticket, attraction);

        ok.Should().BeFalse();

        _mockEventRepository.VerifyAll();
    }

    [TestMethod]
    public void ValidateEventEntry_WhenAttractionInEvent_WithinWindow_AndCapacityAvailable_ShouldReturnTrue()
    {
        var attraction = new Attraction { Id = Guid.NewGuid(), Available = true, Capacity = 50, CurrentVisitors = 0 };

        var ev = new Event
        {
            Id = Guid.NewGuid(),
            Name = "Show",
            Date = _now,
            Capacity = 10,
            Cost = 0,
            Attractions = [attraction]
        };

        var ticket = new Ticket
        {
            Date = _now,
            Type = EntranceType.Event,
            Event = ev,
            EventId = ev.Id,
            Visitor = new VisitorProfile { Id = Guid.NewGuid() },
            VisitorProfileId = Guid.NewGuid()
        };

        _mockEventRepository
            .Setup(r => r.Get(e => e.Id == ticket.EventId))
            .Returns(ev);

        _mockTicketRepository
            .Setup(r => r.GetAll(t => t.EventId == ticket.EventId))
            .Returns([]);

        _mockAttractionRepository
            .Setup(r => r.Update(attraction));

        var ok = _attractionService.ValidateEventEntry(ticket, attraction);

        ok.Should().BeTrue();

        _mockEventRepository.VerifyAll();
        _mockTicketRepository.VerifyAll();
        _mockAttractionRepository.VerifyAll();
        _mockClock.VerifyAll();
    }
    #endregion
}
