using FluentAssertions;
using VirtualPark.BusinessLogic.Rewards.Models;

namespace VirtualPark.BusinessLogic.Test.Reward.Models;

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

        var args = new RewardArgs(name, "Exclusive access", "100");

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
            var rewardArgs = new RewardArgs(invalidName!, "Exclusive access", "100");
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
        var args = new RewardArgs("VIP Ticket", "Exclusive access", "100");
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
            var rewardArgs = new RewardArgs("VIP Ticket", invalidDescription!, "100");
        };
        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Cost
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenCostIsValid_ShouldSetCost()
    {
        var args = new RewardArgs("VIP Ticket", "desc", "100");
        args.Cost.Should().Be(100);
    }
    #endregion
}
