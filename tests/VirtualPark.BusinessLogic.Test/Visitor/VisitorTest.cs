using FluentAssertions;
using VirtualPark.BusinessLogic.Visitors.Entity;

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
        var visitor = new Visitor("Name");

        // Assert
        visitor.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void SetDateOfBirth_WhenValueIsInFuture_ShouldThrowArgumentException()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor("Name");
            visitor.DateOfBirth = futureDate;
        });

        // Assert
        Assert.AreEqual("Date of birth cannot be in the future", ex.Message);
    }

    [DataTestMethod]
    [TestCategory("Validation")]
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    public void SetName_WhenValueIsNullOrEmpty_ShouldThrowArgumentException(string invalidName)
    {
        // Act
        var ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor(invalidName);
        });

        // Assert
        Assert.AreEqual("Name cannot be null or empty", ex.Message);
    }

    [DataTestMethod]
    [TestCategory("Validation")]
    [DataRow("not-an-email")]
    [DataRow("missingatsign.com")]
    [DataRow("user@.com")]
    public void Email_WhenInvalidFormat_ShouldThrowArgumentException(string invalidEmail)
    {
        // Act
        Action act = () => new Visitor { Email = invalidEmail };

        // Assert
        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Email format is invalid");
    }
}
