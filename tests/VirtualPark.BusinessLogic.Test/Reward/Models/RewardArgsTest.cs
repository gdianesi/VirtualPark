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

        var args = new RewardArgs(name, "Exclusive access");

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
            var rewardArgs = new RewardArgs(invalidName!, "Exclusive access");
        };

        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty.");
    }
    #endregion
    #endregion

    #region Description
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenDescriptionIsValid_ShouldSetDescription()
    {
        var args = new RewardArgs("VIP Ticket", "Exclusive access");
        args.Description.Should().Be("Exclusive access");
    }
    #endregion
}
