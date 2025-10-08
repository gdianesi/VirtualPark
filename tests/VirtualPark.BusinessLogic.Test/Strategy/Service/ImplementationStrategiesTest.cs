using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Attractions.Entity;
using VirtualPark.BusinessLogic.Events.Entity;
using VirtualPark.BusinessLogic.Strategy.Services;
using VirtualPark.BusinessLogic.Tickets.Entity;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;
using VirtualPark.BusinessLogic.VisitRegistrations.Entity;

namespace VirtualPark.BusinessLogic.Test.Strategy.Service;

[TestClass]
public sealed class ImplementationStrategiesTest
{
    #region AttractionPointsStrategy
    [TestMethod]
    public void AttractionPoints_ShouldBeZero_WhenNoAttractions()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration { Attractions = [] };
        strategy.CalculatePoints(visit).Should().Be(0);
    }

    [TestMethod]
    public void AttractionPoints_RollerCoaster()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration { Attractions = [new() { Type = AttractionType.RollerCoaster }] };
        strategy.CalculatePoints(visit).Should().Be(50);
    }

    [TestMethod]
    public void AttractionPoints_Show()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration { Attractions = [new() { Type = AttractionType.Show }] };
        strategy.CalculatePoints(visit).Should().Be(30);
    }

    [TestMethod]
    public void AttractionPoints_Simulator()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration { Attractions = [new() { Type = AttractionType.Simulator }] };
        strategy.CalculatePoints(visit).Should().Be(20);
    }

    [TestMethod]
    public void AttractionPoints_UnknownType()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration { Attractions = [new() { Type = (AttractionType)999 }] };
        strategy.CalculatePoints(visit).Should().Be(10);
    }

    [TestMethod]
    public void AttractionPoints_MixedTypes()
    {
        var strategy = new AttractionPointsStrategy();
        var visit = new VisitRegistration
        {
            Attractions =
            [
                new() { Type = AttractionType.RollerCoaster },
                new() { Type = AttractionType.Show },
                new() { Type = AttractionType.Simulator },
                new() { Type = (AttractionType)999 }
            ]
        };
        strategy.CalculatePoints(visit).Should().Be(50 + 30 + 20 + 10);
    }
    #endregion

    #region ComboPointsStrategy
    [TestMethod]
    public void ComboPoints_ShouldBeZero_WhenNoAttractions()
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = [] };
        strategy.CalculatePoints(visit).Should().Be(0);
    }

    [DataTestMethod]
    [DataRow(1, 2)]
    [DataRow(5, 10)]
    [DataRow(10, 20)]
    public void ComboPoints_BaseRule(int count, int expected)
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = new List<Attraction>(new Attraction[count]) };
        strategy.CalculatePoints(visit).Should().Be(expected);
    }

    [DataTestMethod]
    [DataRow(11, 32)]
    [DataRow(15, 40)]
    [DataRow(25, 60)]
    public void ComboPoints_BonusRule(int count, int expected)
    {
        var strategy = new ComboPointsStrategy();
        var visit = new VisitRegistration { Attractions = new List<Attraction>(new Attraction[count]) };
        strategy.CalculatePoints(visit).Should().Be(expected);
    }
    #endregion

    #region EventPointsStrategy
    [TestMethod]
    public void EventPoints_ShouldBeZero_WhenNoEvent()
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            Visitor = new VisitorProfile { Score = 100 },
            Ticket = new Ticket { Event = null },
            Attractions = []
        };
        strategy.CalculatePoints(visit).Should().Be(0);
    }

    [DataTestMethod]
    [DataRow(0, 0)]
    [DataRow(10, 30)]
    [DataRow(25, 75)]
    public void EventPoints_ShouldBeTriple_WhenEventPresent(int score, int expected)
    {
        var strategy = new EventPointsStrategy();
        var visit = new VisitRegistration
        {
            Visitor = new VisitorProfile { Score = score },
            Ticket = new Ticket { Event = new Event() },
            Attractions = []
        };
        strategy.CalculatePoints(visit).Should().Be(expected);
    }
    #endregion
}
