using System.Linq.Expressions;
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
            _mockUserReadOnlyRepository.Object
        );
    }

    #region GuidToUser
    [TestMethod]
    public void GuidToUser_WhenUserDoesNotExist_ShouldThrowKeyNotFound()
    {
        var g1 = Guid.NewGuid();
        _mockUserReadOnlyRepository.Setup(r => r.Get(u => u.Id == g1)).Returns((User?)null);

        var entries = new List<Guid> { g1 };

        Action act = () => _rankingService.GuidToUser(entries);

        act.Should().Throw<KeyNotFoundException>()
            .WithMessage($"User with id {g1} does not exist");
    }

    [TestMethod]
    public void GuidToUser_WhenEntriesIsNull_ShouldThrowArgumentNull()
    {
        Action act = () => _rankingService.GuidToUser(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    #endregion
    #region MapToEntity
    [TestMethod]
    public void MapToEntity_WhenArgsAreValid_ShouldReturnRankingEntity()
    {
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString(), g2.ToString() }, "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };
        var user2 = new User { Name = "Bob", LastName = "Jones", Email = "b@test.com", Password = "456", Roles = [] };

        var queue = new Queue<User>(new[] { user1, user2 });

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(() => queue.Dequeue());

        var ranking = _rankingService.MapToEntity(args);

        ranking.Should().NotBeNull();
        ranking.Date.Should().Be(args.Date);
        ranking.Period.Should().Be(args.Period);
        ranking.Entries.Should().HaveCount(2);
    }

    #endregion

    #region ApllyArgsToEntity
    [TestMethod]
        public void ApplyArgsToEntity_ValidArgs_ShouldCopyFieldsAndMapEntries()
        {
            var g1 = Guid.NewGuid();
            var g2 = Guid.NewGuid();
            var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString(), g2.ToString() }, "Daily");

            var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };
            var user2 = new User { Name = "Bob",   LastName = "Jones", Email = "b@test.com", Password = "456", Roles = [] };

            var queue = new Queue<User>(new[] { user1, user2 });
            _mockUserReadOnlyRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => queue.Dequeue());

            var entity = new Ranking();

            _rankingService.ApplyArgsToEntity(entity, args);

            Assert.AreEqual(args.Date, entity.Date);
            Assert.AreEqual(args.Period, entity.Period);
            Assert.IsNotNull(entity.Entries);
            Assert.AreEqual(2, entity.Entries.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void ApplyArgsToEntity_WhenAGuidDoesNotResolve_ShouldThrowKeyNotFound()
        {
            var g1 = Guid.NewGuid();
            var gMissing = Guid.NewGuid();
            var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString(), gMissing.ToString() }, "Daily");

            var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };

            var step = 0;
            _mockUserReadOnlyRepository
                .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(() => step++ == 0 ? user1 : null);

            var entity = new Ranking();

            _rankingService.ApplyArgsToEntity(entity, args);
        }
    #endregion

    #region Create
    [TestMethod]
    public void Create_WhenArgsValid_ShouldAddAndReturnId()
    {
        var g1 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString() }, "Daily");

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "123", Roles = [] };

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(user1);

        _mockRankingRepository
            .Setup(r => r.Add(It.IsAny<Ranking>()));

        var id = _rankingService.Create(args);

        id.Should().NotBe(Guid.Empty);
        _mockRankingRepository.Verify(r => r.Add(It.IsAny<Ranking>()), Times.Once);
        _mockUserReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
        _mockUserReadOnlyRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Create_WhenUserNotFound_ShouldThrow()
    {
        var g1 = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString() }, "Daily");

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns((User?)null);

        Action act = () => _rankingService.Create(args);

        act.Should().Throw<KeyNotFoundException>();
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
            .Returns(new List<Ranking> { r1, r2 });

        var result = _rankingService.GetAll();

        result.Should().HaveCount(2);
        result.Should().ContainInOrder(r1, r2);
        _mockRankingRepository.Verify(r => r.GetAll(null), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void GetAll_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        _mockRankingRepository
            .Setup(r => r.GetAll(null))
            .Returns(new List<Ranking>());

        var result = _rankingService.GetAll();

        result.Should().BeEmpty();
        _mockRankingRepository.Verify(r => r.GetAll(null), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }
    #endregion
    #region Get
    [TestMethod]
    public void Get_WhenRankingExists_ShouldReturnRanking()
    {
        var ranking = new Ranking
        {
            Date = new DateTime(2025, 9, 27),
            Period = Period.Daily
        };

        _mockRankingRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns(ranking);

        var result = _rankingService.Get(r => r.Period == Period.Daily);

        result.Should().NotBeNull();
        result.Should().BeSameAs(ranking);
        _mockRankingRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Get_WhenRankingDoesNotExist_ShouldReturnNull()
    {
        _mockRankingRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns((Ranking?)null);

        var result = _rankingService.Get(r => r.Period == Period.Weekly);

        result.Should().BeNull();
        _mockRankingRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }
    #endregion
    #region Exist
    [TestMethod]
    public void Exist_WhenRepositoryReturnsTrue_ShouldReturnTrue()
    {
        _mockRankingRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns(true);

        var result = _rankingService.Exist(r => r.Period == Period.Daily);

        result.Should().BeTrue();
        _mockRankingRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Exist_WhenRepositoryReturnsFalse_ShouldReturnFalse()
    {
        _mockRankingRepository
            .Setup(r => r.Exist(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns(false);

        var result = _rankingService.Exist(r => r.Period == Period.Weekly);

        result.Should().BeFalse();
        _mockRankingRepository.Verify(r => r.Exist(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockRankingRepository.VerifyNoOtherCalls();
    }
    #endregion
    #region Update
    [TestMethod]
    public void Update_WhenRankingExists_ShouldApplyArgsAndCallRepositoryUpdate()
    {
        var id = Guid.NewGuid();
        var g1 = Guid.NewGuid();
        var g2 = Guid.NewGuid();

        var args = new RankingArgs("2025-09-27 00:00", new[] { g1.ToString(), g2.ToString() }, "Daily");

        var existing = new Ranking
        {
            Id = id,
            Date = new DateTime(2024, 1, 1),
            Period = Period.Weekly,
            Entries = new List<User>()
        };

        _mockRankingRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns(existing);

        var user1 = new User { Name = "Alice", LastName = "Smith", Email = "a@test.com", Password = "x", Roles = [] };
        var user2 = new User { Name = "Bob",   LastName = "Jones",  Email = "b@test.com", Password = "y", Roles = [] };
        var queue = new Queue<User>(new[] { user1, user2 });

        _mockUserReadOnlyRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()))
            .Returns(() => queue.Dequeue());

        _mockRankingRepository
            .Setup(r => r.Update(It.IsAny<Ranking>()));

        _rankingService.Update(args, id);

        existing.Entries.Should().HaveCount(2);

        _mockRankingRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockUserReadOnlyRepository.Verify(r => r.Get(It.IsAny<Expression<Func<User, bool>>>()), Times.Exactly(2));
        _mockRankingRepository.Verify(r => r.Update(existing), Times.Once);

        _mockRankingRepository.VerifyNoOtherCalls();
        _mockUserReadOnlyRepository.VerifyNoOtherCalls();
    }

    [TestMethod]
    public void Update_WhenRankingDoesNotExist_ShouldThrowInvalidOperation()
    {
        var id = Guid.NewGuid();
        var args = new RankingArgs("2025-09-27 00:00", Array.Empty<string>(), "Daily");

        _mockRankingRepository
            .Setup(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()))
            .Returns((Ranking?)null);

        Action act = () => _rankingService.Update(args, id);

        act.Should().Throw<InvalidOperationException>()
           .WithMessage($"Ranking with id {id} not found.");

        _mockRankingRepository.Verify(r => r.Get(It.IsAny<Expression<Func<Ranking, bool>>>()), Times.Once);
        _mockRankingRepository.Verify(r => r.Update(It.IsAny<Ranking>()), Times.Never);
        _mockRankingRepository.VerifyNoOtherCalls();
        _mockUserReadOnlyRepository.VerifyNoOtherCalls();
    }
    #endregion

}
