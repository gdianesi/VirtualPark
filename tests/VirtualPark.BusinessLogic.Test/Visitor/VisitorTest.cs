using FluentAssertions;

namespace VirtualPark.BusinessLogic.Test.Visitor;

[TestClass]
[TestCategory("Entity")]
[TestCategory("Visitor")]
public class VisitorTest
{
    [TestMethod]
    [TestCategory("Constructor")]
    public void WhenVisitorIsCreated_IdShouldBeAssigned()
    {
        // Act
        var visitor = new Visitors.Entity.Visitor();

        // Assert
        visitor.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void Visitor_Birthday_ShouldBeInvalid_WhenSetInFuture()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(
            () => new Visitors.Entity.Visitor { DateOfBirth = futureDate },
            "Date of birth cannot be in the future.");
    }
}
