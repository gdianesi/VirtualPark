using FluentAssertions;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Test.Validations;

[TestClass]
[TestCategory("Validations")]
public class ValidationServicesTest
{
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
}
