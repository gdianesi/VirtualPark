using FluentAssertions;
using VirtualPark.BusinessLogic.Rewards.Models;

namespace VirtualPark.BusinessLogic.Test.Reward.Models;

[TestClass]
[TestCategory("Args")]
public sealed class RewardArgsTest
{
    #region Name
    [TestMethod]
    [TestCategory("Validation")]
    public void Constructor_WhenNameIsValid_ShouldSetName()
    {
        const string name = "VIP Ticket";

        var args = new RewardArgs(name);

        args.Name.Should().Be(name);
    }
    #endregion

    [DataTestMethod]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [TestCategory("Validation")]
    public void Constructor_WhenNameIsNullOrEmpty_ShouldThrowArgumentException(string? invalidName)
    {
        Action act = () =>
        {
            var rewardArgs = new RewardArgs(invalidName!);
        };

        act.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty.");
    }
}
