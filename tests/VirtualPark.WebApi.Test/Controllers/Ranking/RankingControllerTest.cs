using FluentAssertions;
using Moq;
using VirtualPark.BusinessLogic.Rankings;
using VirtualPark.BusinessLogic.Rankings.Models;
using VirtualPark.BusinessLogic.Rankings.Service;
using VirtualPark.BusinessLogic.Users.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.WebApi.Controllers.Ranking;
using VirtualPark.WebApi.Controllers.Ranking.ModelsIn;
using VirtualPark.WebApi.Controllers.Ranking.ModelsOut;

namespace VirtualPark.WebApi.Test.Controllers.Ranking;

[TestClass]
[TestCategory("Controller")]
[TestCategory("RankingController")]
public sealed class RankingControllerTest
{
    private Mock<IRankingService> _rankingServiceMock = null!;
    private RankingController _controller = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rankingServiceMock = new(MockBehavior.Strict);
        _controller = new RankingController(_rankingServiceMock.Object);
    }

    #region Get
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetRanking_WhenValidRequest_ShouldReturnGetRankingResponse()
    {
        var users = new List<User>
        {
            new() { Name = "Juan" },
            new() { Name = "Ana" }
        };

        var ranking = new BusinessLogic.Rankings.Entity.Ranking
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 10, 06),
            Period = Period.Daily,
            Entries = users
        };

        var request = new GetRankingRequest
        {
            Date = "2025-10-06",
            Period = "Daily"
        };

        var expectedArgs = request.ToArgs();

        _rankingServiceMock
            .Setup(s => s.Get(It.Is<RankingArgs>(a =>
                a.Date == expectedArgs.Date &&
                a.Period == expectedArgs.Period)))
            .Returns(ranking);

        var result = _controller.GetRanking(request);

        result.Should().NotBeNull();
        result.Should().BeOfType<GetRankingResponse>();
        result.Id.Should().Be(ranking.Id.ToString());
        result.Date.Should().Be(ranking.Date.ToString("yyyy-MM-dd"));
        result.Period.Should().Be("Daily");
        result.Users.Should().BeEquivalentTo(ranking.Entries.Select(u => u.Id.ToString()).ToList());

        _rankingServiceMock.VerifyAll();
    }

    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetRanking_WhenUserHasAndHasNotVisitorProfile_ShouldMapScoresCorrectly()
    {
        var userWithProfile = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfile = new VisitorProfile()
            {
                Score = 50
            }
        };

        var userWithoutProfile = new User
        {
            Id = Guid.NewGuid(),
            VisitorProfile = null
        };

        var ranking = new BusinessLogic.Rankings.Entity.Ranking
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 11, 7),
            Period = Period.Daily,
            Entries = [userWithProfile, userWithoutProfile]
        };

        var request = new GetRankingRequest
        {
            Date = "2025-11-07",
            Period = "Daily"
        };

        var expectedArgs = request.ToArgs();

        _rankingServiceMock
            .Setup(s => s.Get(It.Is<RankingArgs>(a =>
                a.Date == expectedArgs.Date &&
                a.Period == expectedArgs.Period)))
            .Returns(ranking);

        var result = _controller.GetRanking(request);

        result.Should().NotBeNull();
        result.Scores.Should().Contain("50");
        result.Scores.Should().Contain("0");
        result.Users.Should().HaveCount(2);

        _rankingServiceMock.VerifyAll();
    }
    #endregion

    #region GetAll
    [TestMethod]
    [TestCategory("Behaviour")]
    public void GetAllRankings_ShouldReturnListOfGetRankingResponse()
    {
        var user1 = new User { Name = "Pedro" };
        var user2 = new User { Name = "Lucía" };

        var ranking1 = new BusinessLogic.Rankings.Entity.Ranking
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 10, 05),
            Period = Period.Daily,
            Entries = [user1]
        };

        var ranking2 = new BusinessLogic.Rankings.Entity.Ranking
        {
            Id = Guid.NewGuid(),
            Date = new DateTime(2025, 10, 06),
            Period = Period.Weekly,
            Entries = [user2]
        };

        _rankingServiceMock
            .Setup(s => s.GetAll())
            .Returns([ranking1, ranking2]);

        var result = _controller.GetAllRankings();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        var first = result.First();
        first.Date.Should().Be("2025-10-05");
        first.Period.Should().Be("Daily");

        var second = result.Last();
        second.Date.Should().Be("2025-10-06");
        second.Period.Should().Be("Weekly");

        _rankingServiceMock.VerifyAll();
    }
    #endregion
}
