using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Validations.Services;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Test.Validations;

[TestClass]
[TestCategory("Validations")]
public class ValidationServicesTest
{
    #region ParseInt
    [TestMethod]
    [TestCategory("Validations")]
    public void ParseToInt_WhenInputIsValid_ShouldReturnInteger()
    {
        var number = ValidationServices.ValidateAndParseInt("123");
        number.Should().Be(123);
    }

    [TestMethod]
    [TestCategory("Validations")]
    public void ParseToInt_WhenInputIsNullOrEmpty_ShouldThrowArgumentException()
    {
        var input = string.Empty;

        Action act = () => ValidationServices.ValidateAndParseInt(input);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.*");
    }

    [TestMethod]
    [TestCategory("Validations")]
    public void ParseToInt_WhenInputIsNotNumeric_ShouldThrowFormatException()
    {
        var input = "abc";

        Action act = () => ValidationServices.ValidateAndParseInt(input);

        act.Should()
            .Throw<FormatException>()
            .WithMessage("The value 'abc' is not a valid integer.");
    }
    #endregion

    #region ParseBool

    [TestClass]
    [TestCategory("Validations")]
    public class ValidationBoolTest
    {
        [TestMethod]
        [TestCategory("Validations")]
        public void ParseToBool_WhenInputIsValidTrue_ShouldReturnTrue()
        {
            var result = ValidationServices.ValidateAndParseBool("true");

            result.Should().BeTrue();
        }

        [TestMethod]
        [TestCategory("Validations")]
        public void ParseToBool_WhenInputIsValidFalse_ShouldReturnFalse()
        {
            var result = ValidationServices.ValidateAndParseBool("false");

            result.Should().BeFalse();
        }

        [TestMethod]
        [TestCategory("Validations")]
        public void ParseToBool_WhenInputIsNullOrEmpty_ShouldThrowArgumentException()
        {
            var input = string.Empty;

            Action act = () => ValidationServices.ValidateAndParseBool(input);

            act.Should()
                .Throw<ArgumentException>()
                .WithMessage("Value cannot be null or empty.*");
        }

        [TestMethod]
        [TestCategory("Validations")]
        public void ParseToBool_WhenInputIsNotBoolean_ShouldThrowFormatException()
        {
            var input = "maybe";

            Action act = () => ValidationServices.ValidateAndParseBool(input);

            act.Should()
                .Throw<FormatException>()
                .WithMessage("The value 'maybe' is not a valid boolean.");
        }
    }
    #endregion

    #region ParseGuid
    [TestMethod]
    [TestCategory("Validations")]
    public void ValidateAndParseGuid_WhenInputIsValid_ShouldReturnGuid()
    {
        var input = "f3f0a7c6-2f2d-4b44-9b1f-3f3a4a6a9a10";
        var expected = new Guid(input);

        var result = ValidationServices.ValidateAndParseGuid(input);

        result.Should().Be(expected);
    }

    [TestMethod]
    [TestCategory("Validations")]
    public void ValidateAndParseGuid_WhenInputIsNullOrEmpty_ShouldThrowArgumentException()
    {
        var input = string.Empty;

        Action act = () => ValidationServices.ValidateAndParseGuid(input);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("Value cannot be null or empty.");
    }

    [TestMethod]
    [TestCategory("Validations")]
    public void ValidateAndParseGuid_WhenInputIsInvalid_ShouldThrowFormatException()
    {
        var input = "abc";

        Action act = () => ValidationServices.ValidateAndParseGuid(input);

        act.Should()
            .Throw<FormatException>()
            .WithMessage("The value 'abc' is not a valid GUID.");
    }

    #endregion

    #region ParseAttractionTypeEnum
    [DataTestMethod]
    [DataRow("RollerCoaster", AttractionType.RollerCoaster)]
    [DataRow("rollercoaster", AttractionType.RollerCoaster)]
    [DataRow(" SIMULATOR ", AttractionType.Simulator)]
    [DataRow("show", AttractionType.Show)]
    public void ValidateAndParseAttractionType_ShouldReturnEnum_WhenValueIsValid(
        string input, AttractionType expected)
    {
        var result = ValidationServices.ValidateAndParseAttractionType(input);

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void ValidateAndParseAttractionType_ShouldThrow_WhenValueIsUnknown()
    {
        var input = "FerrisWheel";

        var ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            ValidationServices.ValidateAndParseAttractionType(input);
        });

        StringAssert.Contains(ex.Message, "not a valid AttractionType");
    }

    [TestMethod]
    public void ValidateAndParseAttractionType_ShouldThrow_WhenValueIsNull()
    {
        string? input = null;

        var ex = Assert.ThrowsException<ArgumentException>(
            (Action)(() => ValidationServices.ValidateAndParseAttractionType(input!)));

        StringAssert.Contains(ex.Message, "cannot be null or empty");
    }

    [DataTestMethod]
    [DataRow("")]
    [DataRow("   ")]
    public void ValidateAndParseAttractionType_ShouldThrow_WhenValueIsEmptyOrWhitespace(string input)
    {
        var ex = Assert.ThrowsException<ArgumentException>(() =>
        {
            ValidationServices.ValidateAndParseAttractionType(input);
        });

        StringAssert.Contains(ex.Message, "cannot be null or empty");
    }
    #endregion

    #region ValidateAge
    [DataTestMethod]
    [DataRow(1)]
    [DataRow(18)]
    [DataRow(50)]
    [DataRow(99)]
    public void ValidateAge_WhenAgeIsValid_ShouldNotThrow(int age)
    {
        Action act = () => ValidationServices.ValidateAge(age);
        act.Should().NotThrow();
    }

    [TestMethod]
    public void ValidateAge_WhenAgeIsZero_ShouldThrow()
    {
        Action act = () => ValidationServices.ValidateAge(0);

        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithParameterName("age")
            .WithMessage("*between 1 and 99*");
    }

    [DataTestMethod]
    [DataRow(-1)]
    [DataRow(-10)]
    [DataRow(-100)]
    public void ValidateAge_WhenAgeIsNegative_ShouldThrow(int age)
    {
        Action act = () => ValidationServices.ValidateAge(age);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [DataTestMethod]
    [DataRow(100)]
    [DataRow(150)]
    [DataRow(999)]
    public void ValidateAge_WhenAgeIsGreaterThanOrEqual100_ShouldThrow(int age)
    {
        Action act = () => ValidationServices.ValidateAge(age);
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    #endregion

    #region ValidateEmail
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ValidateEmail_WithValidEmail_ReturnsSameEmail()
    {
        var email = "test.user@mail.com";

        var result = ValidationServices.ValidateEmail(email);

        result.Should().Be(email);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ValidateEmail_WithInvalidEmail_ThrowsArgumentException()
    {
        var invalidEmail = "invalidEmail";

        Action act = () => ValidationServices.ValidateEmail(invalidEmail);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage($"*Invalid email format*")
            .And.ParamName.Should().Be("email");
    }
    #endregion
    #endregion

    #region ValidatePassword
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ValidatePassword_WithValidPassword_ReturnsSamePassword()
    {
        var password = "Password123!";

        var result = ValidationServices.ValidatePassword(password);

        result.Should().Be(password);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ValidatePassword_WithInvalidPassword_ThrowsArgumentException()
    {
        var invalidPassword = "pass";

        Action act = () => ValidationServices.ValidatePassword(invalidPassword);

        act.Should()
            .Throw<ArgumentException>()
            .WithMessage("*Password must be at least 8 characters long*")
            .And.ParamName.Should().Be("password");
    }
    #endregion
    #endregion

    #region ParseDateOfBirth
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ParseDateOfBirth_ShouldReturnDate_WhenFormatIsValid()
    {
        var input = "2002-07-30";

        var result = ValidationServices.ParseDateOfBirth(input);

        result.Year.Should().Be(2002);
        result.Month.Should().Be(7);
        result.Day.Should().Be(30);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ParseDateOfBirth_ShouldThrow_WhenFormatIsInvalid()
    {
        var input = "30-07-2002";

        Action act = () => ValidationServices.ParseDateOfBirth(input);

        act.Should().Throw<ArgumentException>()
            .WithMessage("Invalid date format: 30-07-2002. Expected format is yyyy-MM-dd*")
            .And.ParamName.Should().Be("dateOfBirth");
    }
    #endregion
    #endregion

    #region ParseMembership
    #region Success
    [TestMethod]
    [TestCategory("Validation")]
    public void ParseMembership_ShouldReturnEnum_WhenValueIsValidIgnoringCase()
    {
        var input = "Standard";

        var result = ValidationServices.ParseMembership(input);

        result.Should().Be(Membership.Standard);
    }
    #endregion

    #region Failure
    [TestMethod]
    [TestCategory("Validation")]
    public void ParseMembership_ShouldThrow_WhenValueIsInvalid()
    {
        var input = "InvalidValue";

        Action act = () => ValidationServices.ParseMembership(input);

        var ex = act.Should().Throw<ArgumentException>().Which;
        ex.ParamName.Should().Be("membership");
        ex.Message.Should().StartWith("Invalid membership value: InvalidValue");
    }
    #endregion
    #endregion
}
