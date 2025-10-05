using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rankings;
using VirtualPark.BusinessLogic.Rankings.Entity;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.Repository;

namespace VirtualPark.BusinessLogic.Test.Rankings.Service;

[TestClass]
[TestCategory("Rankings")]
public sealed class RankingServiceTest
{
    private Mock<IRepository<Ranking>> _mockRankingRepository = null!;
    private Mock<IReadOnlyRepository<User>> _mockUserReadOnlyRepository = null!;
    private RankingService _rankingService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockRankingRepository = new Mock<IRepository<Ranking>>(MockBehavior.Strict);
        _mockUserReadOnlyRepository = new Mock<IReadOnlyRepository<User>>(MockBehavior.Strict);

        _rankingService = new RankingService(
            _mockRankingRepository.Object,
            _mockUserReadOnlyRepository.Object);
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
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString(), g2.ToString()], "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };
        var user2 = new User { Name = "Bob", LastName = "Jones", Email = "b@test.com", Password = "456", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns(user1);
        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g2))
            .Returns(user2);

        var ranking = _rankingService.MapToEntity(args);

        ranking.Should().NotBeNull();
        ranking.Date.Should().Be(args.Date);
        ranking.Period.Should().Be(args.Period);
        ranking.Entries.Should().HaveCount(2);
        ranking.Entries[0].Should().BeSameAs(user1);
        ranking.Entries[1].Should().BeSameAs(user2);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region ApplyArgsToEntity
    [TestMethod]
    public void ApplyArgsToEntity_ValidArgs_ShouldCopyFieldsAndMapEntries()
    {
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString(), g2.ToString()], "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };
        var user2 = new User { Name = "Bob", LastName = "Jones", Email = "b@test.com", Password = "456", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns(user1);
        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g2))
            .Returns(user2);

        var entity = new Ranking();

        _rankingService.ApplyArgsToEntity(entity, args);

        entity.Date.Should().Be(args.Date);
        entity.Period.Should().Be(args.Period);
        entity.Entries.Should().NotBeNull();
        entity.Entries.Should().HaveCount(2);
        entity.Entries[0].Should().BeSameAs(user1);
        entity.Entries[1].Should().BeSameAs(user2);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }

    [TestMethod]
    public void ApplyArgsToEntity_WhenAGuidDoesNotResolve_ShouldThrowKeyNotFound()
    {
        var g1 = Guid.NewGuid();
        var gMissing = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString(), gMissing.ToString()], "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns(user1);
        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == gMissing))
            .Returns((User?)null);

        var entity = new Ranking();

        var act = () => _rankingService.ApplyArgsToEntity(entity, args);

        act.Should().Throw<KeyNotFoundException>();

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }
    #endregion

    #region Create
    [TestMethod]
    public void Create_WhenArgsValid_ShouldAddAndReturnId()
    {
        var g1 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString()], "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns(user1);

        _mockRankingRepository
            .Setup(r => r.Add(It.Is<Ranking>(rk =>
                rk.Date == args.Date &&
                rk.Period == args.Period &&
                rk.Entries.Count == 1 &&
                rk.Entries[0] == user1)));

        var id = _rankingService.Create(args);

        id.Should().NotBe(Guid.Empty);

        _mockUserReadOnlyRepository.VerifyAll();
        _mockRankingRepository.VerifyAll();
    }

    [TestMethod]
    public void Create_WhenUserNotFound_ShouldThrow()
    {
        var g1 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString()], "Daily");

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns((User?)null);

        var act = () => _rankingService.Create(args);

        act.Should().Throw<KeyNotFoundException>();

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
            .Setup(r => r.GetAll(null))
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
            .Setup(r => r.GetAll(null))
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
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();

        var args = new RankingArgs("2025-09-27 00:00", [g1.ToString(), g2.ToString()], "Daily");

        var existing = new Ranking
        {
            Id = id,
            Date = new DateTime(2024, 1, 1),
            Period = Period.Weekly,
            Entries = []
        };

        _mockRankingRepository
            .Setup(r => r.Get(rk => rk.Id == id))
            .Returns(existing);

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "x", Roles = [] };
        var user2 = new User { Name = "Bob", LastName = "Jones", Email = "b@test.com", Password = "y", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g1))
            .Returns(user1);
        _mockUserReadOnlyRepository
            .Setup(r => r.Get(u => u.Id == g2))
            .Returns(user2);

        _mockRankingRepository
            .Setup(r => r.Update(It.Is<Ranking>(rk =>
                rk.Id == id &&
                rk.Date == args.Date &&
                rk.Period == args.Period &&
                rk.Entries.Count == 2 &&
                rk.Entries[0] == user1 &&
                rk.Entries[1] == user2)));

        _rankingService.Update(args, id);

        _mockRankingRepository.VerifyAll();
        _mockUserReadOnlyRepository.VerifyAll();
    }

    [TestMethod]
    public void Update_WhenRankingDoesNotExist_ShouldThrowInvalidOperation()
    {
        var id = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", [], "Daily");

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
}
