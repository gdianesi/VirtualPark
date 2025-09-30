using System.Linq.Expressions;
using FluentAssertions;
using Moq;
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
}
