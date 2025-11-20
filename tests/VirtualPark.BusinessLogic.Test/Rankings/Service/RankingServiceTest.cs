using System.Linq.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using VirtualPark.BusinessLogic.Rankings;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Rankings.Service;

[TestClass]
[TestCategory("Rankings")]
public sealed class RankingServiceTest
{
    private Mock<IRepository<Ranking>> _mockRankingRepository = null!;
    private Mock<IReadOnlyRepository<User>> _mockUserReadOnlyRepository = null!;
    private RankingService _rankingService = null!;
    private Mock<IReadOnlyRepository<VisitRegistration>> _mockVisitRegistrationsReadOnlyRepository = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockRankingRepository = new Mock<IRepository<Ranking>>(MockBehavior.Strict);
        _mockUserReadOnlyRepository = new Mock<IReadOnlyRepository<User>>(MockBehavior.Strict);
        _mockVisitRegistrationsReadOnlyRepository = new Mock<IReadOnlyRepository<VisitRegistration>>(MockBehavior.Strict);

        _rankingService = new RankingService(
            _mockRankingRepository.Object,
            _mockUserReadOnlyRepository.Object,
            _mockVisitRegistrationsReadOnlyRepository.Object);
    }

    #region GuidToUser
    [TestMethod]
    public void GuidToUser_WhenUserDoesNotExist_ShouldThrowKeyNotFound()
    {
        var g1 = Guid.NewGuid();

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns((User?)null);

        List<Guid> entries = [g1];

        Action act = () => _rankingService.GuidToUser(entries);

        act.Should().Throw<KeyNotFoundException>()
            .WithMessage($"User with id {g1} does not exist");

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }

    [TestMethod]
    public void GuidToUser_WhenEntriesIsNull_ShouldThrowArgumentNull()
    {
        Action act = () => _rankingService.GuidToUser(null!);

        act.Should().Throw<ArgumentNullException>();

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region MapToEntity
    [TestMethod]
    public void MapToEntity_WhenArgsAreValid_ShouldReturnRankingEntity()
    {
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        var ranking = _rankingService.MapToEntity(args);

        ranking.Should().NotBeNull();
        ranking.Date.Should().Be(args.Date);
        ranking.Period.Should().Be(args.Period);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region ApplyArgsToEntity
    [TestMethod]
    public void ApplyArgsToEntity_ValidArgs_ShouldCopyFieldsAndMapEntries()
    {
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        var entity = new Ranking();

        _rankingService.ApplyArgsToEntity(entity, args);

        entity.Date.Should().Be(args.Date);
        entity.Period.Should().Be(args.Period);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region Create
    [TestMethod]
    public void Create_WhenArgsValid_ShouldAddAndReturnId()
    {
        var g1 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        _mockRankingRepository
            .Setup(r => r.Add(It.Is<Ranking>(rk =>
                rk.Date == args.Date &&
                rk.Period == args.Period)));

        var id = _rankingService.Create(args);

        id.Should().NotBe(Guid.Empty);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    public void GetAll_WhenRepositoryReturnsRankings_ShouldReturnSameList()
    {
        var r1 = new Ranking { Date = new DateTime(2025, 9, 27), Period = Period.Daily };
        var r2 = new Ranking { Date = new DateTime(2025, 9, 28), Period = Period.Weekly };

        _mockRankingRepository
            .Setup(r => r.GetAll())
            .Returns([r1, r2]);

        var result = _rankingService.GetAll();

        result.Should().HaveCount(2);
        result.Should().ContainInOrder(r1, r2);

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        _mockRankingRepository
            .Setup(r => r.GetAll())
            .Returns([]);

        var result = _rankingService.GetAll();

        result.Should().BeEmpty();

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }
    #endregion

    #region Get
    [TestMethod]
    public void Get_WhenRankingExists_ShouldReturnRanking()
    {
        var id = Guid.NewGuid();
        var ranking = new Ranking
        {
            Id = id,
            Date = new DateTime(2025, 9, 27),
            Period = Period.Daily
        };

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns(ranking);

        var result = _rankingService.Get(id);

        result.Should().NotBeNull();
        result.Should().BeSameAs(ranking);

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_WhenRankingDoesNotExist_ShouldThrow()
    {
        var id = Guid.NewGuid();

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns((Ranking?)null);

        Action act = () => _rankingService.Get(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Raking don't exist");

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }
    #endregion

    #region Update
    [TestMethod]
    public void Update_WhenRankingExists_ShouldApplyArgsAndCallRepositoryUpdate()
    {
        var id = Guid.NewGuid();

        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        var existing = new Ranking
        {
            Id = id,
            Date = new DateTime(2024, 1, 1),
            Period = Period.Weekly,
        };

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns(existing);

        _mockRankingRepository
            .Setup(r => r.Update(It.Is<Ranking>(rk =>
                rk.Id == id &&
                rk.Date == args.Date &&
                rk.Period == args.Period)));

        _rankingService.Update(args, id);

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_WhenRankingDoesNotExist_ShouldThrowInvalidOperation()
    {
        var id = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns((Ranking?)null);

        Action act = () => _rankingService.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Raking don't exist");

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }
    #endregion

    #region Remove
    [TestMethod]
    public void Remove_WhenRankingExists_ShouldCallRepositoryRemove()
    {
        var id = Guid.NewGuid();
        var existing = new Ranking
        {
            Id = id,
            Date = new DateTime(2025, 9, 27),
            Period = Period.Daily,
            Entries = []
        };

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns(existing);

        _mockRankingRepository
            .Setup(r => r.Remove(existing));

        _rankingService.Remove(id);

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Remove_WhenRankingDoesNotExist_ShouldThrowInvalidOperation()
    {
        var id = Guid.NewGuid();

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns((Ranking?)null);

        Action act = () => _rankingService.Remove(id);

        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"Raking don't exist");

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }
    #endregion
    #region GetByArgs
    [TestMethod]
    public void Get_WhenRankingDoesNotExist_ShouldCreateWithCalculatedTopUsers_AndReturnRanking()
    {
        var date = new DateTime(2025, 9, 27);
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        var u1 = new User { Name = "Ana" };
        var u2 = new User { Name = "Beto" };
        var u3 = new User { Name = "Cata" };

        u1.VisitorProfile = new VisitorProfile { Id = Guid.NewGuid() };
        u1.VisitorProfileId = u1.VisitorProfile.Id;

        u2.VisitorProfile = new VisitorProfile { Id = Guid.NewGuid() };
        u2.VisitorProfileId = u2.VisitorProfile.Id;

        u3.VisitorProfile = new VisitorProfile { Id = Guid.NewGuid() };
        u3.VisitorProfileId = u3.VisitorProfile.Id;

        var visits = new List<VisitRegistration>
    {
        new() { VisitorId = u1.VisitorProfileId!.Value, Date = date, DailyScore = 10 },
        new() { VisitorId = u1.VisitorProfileId!.Value, Date = date, DailyScore = 20 },
        new() { VisitorId = u2.VisitorProfileId!.Value, Date = date, DailyScore = 5 },
        new() { VisitorId = u2.VisitorProfileId!.Value, Date = date, DailyScore = 15 },
        new() { VisitorId = u3.VisitorProfileId!.Value, Date = date, DailyScore = 10 },
    };

        _mockVisitRegistrationsReadOnlyRepository
            .Setup(r => r.GetAll())
            .Returns(visits);

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns<Expression<Func<User, bool>>, Func<IQueryable<User>, IIncludableQueryable<User, object>>>(
                (expr, include) =>
                {
                    if(expr.Compile().Invoke(u1))
                    {
                        return u1;
                    }

                    if(expr.Compile().Invoke(u2))
                    {
                        return u2;
                    }

                    if(expr.Compile().Invoke(u3))
                    {
                        return u3;
                    }

                    return null!;
                });

        _mockRankingRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Ranking, bool>>>(),
                It.IsAny<Func<IQueryable<Ranking>, IIncludableQueryable<Ranking, object>>>()))
            .Returns((Ranking?)null);

        Ranking? addedRanking = null;
        _mockRankingRepository
            .Setup(r => r.Add(It.IsAny<Ranking>()))
            .Callback<Ranking>(rk => addedRanking = rk);

        var result = _rankingService.Get(args);

        result.Should().NotBeNull();
        addedRanking.Should().NotBeNull();

        addedRanking!.Entries.Should().NotBeEmpty();

        addedRanking.Entries.Select(e => e.Id)
            .Should()
            .ContainInOrder(u1.Id, u2.Id, u3.Id);

        _mockRankingRepository.Verify(r => r.Add(It.IsAny<Ranking>()), Times.Once);
        _mockUserReadOnlyRepository.VerifyAll();
        _mockVisitRegistrationsReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Get_WhenRankingExists_ShouldUpdateEntriesWithCalculatedTopUsers_AndReturnRanking()
    {
        var date = new DateTime(2025, 9, 27);
        var args = new RankingArgs("2025-09-27 00:00", "Daily");

        var u1 = new User { Name = "Ana" };
        var u2 = new User { Name = "Beto" };

        u1.VisitorProfile = new VisitorProfile { Id = Guid.NewGuid() };
        u1.VisitorProfileId = u1.VisitorProfile.Id;

        u2.VisitorProfile = new VisitorProfile { Id = Guid.NewGuid() };
        u2.VisitorProfileId = u2.VisitorProfile.Id;

        var existing = new Ranking
        {
            Id = Guid.NewGuid(),
            Date = date,
            Period = Period.Daily,
            Entries = []
        };

        var visits = new List<VisitRegistration>
    {
        new() { VisitorId = u2.VisitorProfileId!.Value, Date = date, DailyScore = 25 },
        new() { VisitorId = u2.VisitorProfileId!.Value, Date = date, DailyScore = 15 },
        new() { VisitorId = u1.VisitorProfileId!.Value, Date = date, DailyScore = 10 },
    };

        _mockVisitRegistrationsReadOnlyRepository
            .Setup(r => r.GetAll())
            .Returns(visits);

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<User, bool>>>(),
                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()))
            .Returns<Expression<Func<User, bool>>, Func<IQueryable<User>, IIncludableQueryable<User, object>>>(
                (expr, include) =>
                {
                    if(expr.Compile().Invoke(u1))
                    {
                        return u1;
                    }

                    if(expr.Compile().Invoke(u2))
                    {
                        return u2;
                    }

                    return null!;
                });

        _mockRankingRepository
            .Setup(r => r.Get(
                It.IsAny<Expression<Func<Ranking, bool>>>(),
                It.IsAny<Func<IQueryable<Ranking>, IIncludableQueryable<Ranking, object>>>()))
            .Returns(existing);

        _mockRankingRepository
            .Setup(r => r.Update(It.Is<Ranking>(rk =>
                rk.Id == existing.Id &&
                rk.Entries.Count == 2 &&
                rk.Entries[0].Id == u2.Id &&
                rk.Entries[1].Id == u1.Id)))
            .Verifiable();

        var result = _rankingService.Get(args);

        result.Should().BeSameAs(existing);
        result!.Entries.Should().NotBeNull();
        result.Entries.Should().HaveCount(2);
        result.Entries.Select(e => e.Id).Should().ContainInOrder(u2.Id, u1.Id);

        _mockRankingRepository.Verify(r => r.Update(It.IsAny<Ranking>()), Times.Once);
        _mockUserReadOnlyRepository.VerifyAll();
        _mockVisitRegistrationsReadOnlyRepository.VerifyAll();
    }
    #endregion
}
