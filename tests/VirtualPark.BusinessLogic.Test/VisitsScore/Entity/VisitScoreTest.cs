using FluentAssertions;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;
using VirtualPark.BusinessLogic.VisitsScore.Entity;

namespace VirtualPark.BusinessLogic.Test.VisitsScore.Entity;

[TestClass]
[TestCategory("Entity")]
[TestCategory("VisitScore")]
public class VisitScoreTest
{
    #region Id
    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitScoreIsCreated_ShouldAssignId()
    {
        var score = new VisitScore();
        score.Id.Should().NotBe(Guid.Empty);
    }
    #endregion

    #region Origin
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void Origin_Getter_ShouldReturnAssignedInstance()
    {
        var score = new VisitScore { Origin = "Atracción" };

        score.Origin.Should().Be("Atracción");
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void Origin_Setter_ShouldStoreAssignedInstance()
    {
        var score = new VisitScore();
        score.Origin = "Canje";

        score.Origin.Should().Be("Canje");
    }
    #endregion
    #endregion

    #region OccurredAt
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void OccurredAt_Getter_ShouldReturnAssignedInstance()
    {
        var when = new DateTime(2025, 9, 2, 10, 30, 00, DateTimeKind.Utc);
        var score = new VisitScore { OccurredAt = when };

        score.OccurredAt.Should().Be(when);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void OccurredAt_Setter_ShouldStoreAssignedInstance()
    {
        var when = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
        var score = new VisitScore();

        score.OccurredAt = when;

        score.OccurredAt.Should().Be(when);
    }
    #endregion
    #endregion

    #region Points
    #region Get
    [TestMethod]
    [TestCategory("Getter")]
    public void Points_Getter_ShouldReturnAssignedInstance()
    {
        var score = new VisitScore { Points = 50 };

        score.Points.Should().Be(50);
    }
    #endregion

    #region Set
    [TestMethod]
    [TestCategory("Setter")]
    public void Points_Setter_ShouldStorePositiveZeroAndNegativeValues()
    {
        var earn = new VisitScore();
        earn.Points = 100;
        var neutral = new VisitScore();
        neutral.Points = 0;
        var redeem = new VisitScore();
        redeem.Points = -30;

        earn.Points.Should().Be(100);
        neutral.Points.Should().Be(0);
        redeem.Points.Should().Be(-30);
    }
    #endregion
    #endregion

    #region DayStrategyName
    [TestMethod]
    [TestCategory("Getter")]
    public void DayStrategyName_Getter_ShouldReturnAssignedInstance()
    {
        var visit = new VisitScore { DayStrategyName = "Attraction" };

        visit.DayStrategyName.Should().Be("Attraction");
    }

    [TestMethod]
    [TestCategory("Setter")]
    public void DayStrategyName_Setter_ShouldStoreAssignedInstance()
    {
        var visit = new VisitScore();
        visit.DayStrategyName = "Attraction";

        visit.DayStrategyName.Should().Be("Attraction");
    }
    #endregion

    #region VisitRegistrationId
    [TestMethod]
    [TestCategory("GetterSetter")]
    public void VisitRegistrationId_GetSet_ShouldPersistValue()
    {
        var id = Guid.NewGuid();
        var vs = new VisitScore { VisitRegistrationId = id };
        vs.VisitRegistrationId.Should().Be(id);
    }

    [TestMethod]
    [TestCategory("Navigation")]
    public void VisitRegistration_Navigation_ShouldBeAssignable()
    {
        var visit = new VisitRegistration();
        var vs = new VisitScore
        {
            VisitRegistration = visit,
            VisitRegistrationId = visit.Id
        };

        vs.VisitRegistration.Should().BeSameAs(visit);
        vs.VisitRegistrationId.Should().Be(visit.Id);
    }
    #endregion
}
