using FluentAssertions;
using VirtualPark.BusinessLogic.Rewards.Models;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.Rewards.Models;

[TestClass]
[TestCategory("Args")]
public sealed class RewardArgsTest
{
    #region Name
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenNameIsValid_ShouldSetName()
    {
        const string name = "VIP Ticket";

        var args = new RewardArgs(name, "Exclusive access", "100", "5", "VIP");

        args.Name.Should().Be(name);
    }
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [TestCategory("Validation")]
    public void Constructor_WhenNameIsNullOrEmpty_ShouldThrowArgumentException(string? invalidName)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs(invalidName!, "Exclusive access", "100", "5", "VIP");
        };

        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Description
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenDescriptionIsValid_ShouldSetDescription()
    {
        var args = new RewardArgs("VIP Ticket", "Exclusive access", "100", "5", "VIP");
        args.Description.Should().Be("Exclusive access");
    }
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [TestCategory("Validation")]
    public void Constructor_WhenDescriptionIsNullOrEmpty_ShouldThrowArgumentException(string? invalidDescription)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs("VIP Ticket", invalidDescription!, "100", "5", "VIP");
        };
        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Cost
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenCostIsValid_ShouldSetCost()
    {
        var args = new RewardArgs("VIP Ticket", "desc", "100", "5", "VIP");
        args.Cost.Should().Be(100);
    }
    #endregion
    #endregion

    #region Failure
    [DataTestMethod]
    [DataRow("")]
    [DataRow("abc")]
    [DataRow(" ")]
    [TestCategory("Validation")]
    public void Constructor_WhenCostIsInvalid_ShouldThrowException(string invalidCost)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs("VIP Ticket", "desc", invalidCost, "5", "VIP");
        };
        act.Should().Throw<Exception>();
    }
    #endregion

    #region QuantityAvailable
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenQuantityAvailableIsValid_ShouldSetQuantityAvailable()
    {
        var args = new RewardArgs("VIP Ticket", "desc", "100", "5", "VIP");
        args.QuantityAvailable.Should().Be(5);
    }
    #endregion
    #region Failure
    [DataTestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [TestCategory("Validation")]
    public void Constructor_WhenQuantityAvailableIsEmpty_ShouldThrowArgumentException(string invalidQuantity)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs("VIP Ticket", "desc", "100", invalidQuantity, "VIP");
        };
        act.Should().Throw<ArgumentException>();
    }

    [DataTestMethod]
    [DataRow("abc")]
    [TestCategory("Validation")]
    public void Constructor_WhenQuantityAvailableIsNotNumeric_ShouldThrowFormatException(string invalidQuantity)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs("VIP Ticket", "desc", "100", invalidQuantity, "VIP");
        };
        act.Should().Throw<FormatException>();
    }
    #endregion
    #endregion

    #region RequiredMembershipLevel
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenRequiredMembershipLevelIsValid_ShouldSetRequiredMembershipLevel()
    {
        var args = new RewardArgs("VIP Ticket", "desc", "100", "5", "VIP");
        args.RequiredMembershipLevel.Should().Be(Membership.VIP);
    }
    #endregion
}
