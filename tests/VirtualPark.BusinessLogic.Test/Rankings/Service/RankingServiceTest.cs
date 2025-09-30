using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rankings.Entity; using VirtualPark.BusinessLogic.Rankings.Service;
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
}
