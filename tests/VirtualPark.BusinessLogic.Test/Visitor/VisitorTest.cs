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
        var visitor = new Visitor("Name", "visitor@mail.com", "8743b52063cd8");

        // Assert
        visitor.Id.Should().NotBe(Guid.Empty);
    }

    [TestMethod]
    [TestCategory("Validation")]
    public void SetDateOfBirth_WhenValueIsInFuture_ShouldThrowArgumentException()
    {
        // Arrange
        DateTime futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        ArgumentException ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor("Name", "visitor@mail.com", "8743b52063cd8");
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
        ArgumentException ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor(invalidName, "visitor@mail.com", "8743b52063cd8");
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
        ArgumentException ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor("Name", "invalidEmail", "8743b52063cd8");
        });

        // Assert
        Assert.AreEqual("Email format is invalid", ex.Message);
    }

    [TestMethod]
    [DataRow(" ")]
    [DataRow("")]
    [DataRow(null)]
    [TestCategory("Validation")]
    public void Password_WhenEmptyOrNull_ShouldThrowArgumentException(string invalidPassword)
    {
        // Act
        ArgumentException ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            var visitor = new Visitor("Name", "visitor@mail.com", invalidPassword);
        });

        // Assert
        Assert.AreEqual("The password hash cannot be null or empty", ex.Message);
    }

    [TestMethod]
    [TestCategory("Constructor")]
    public void Constructor_WhenVisitorIsCreated_ShouldInitializeScoreAsZero()
    {
        var visitor = new Visitor("Name", "visitor@mail.com", "8743b52063cd8");

        visitor.Score.Should().Be(0);
    }
}
