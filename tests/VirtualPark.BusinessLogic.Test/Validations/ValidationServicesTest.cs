using FluentAssertions;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Validations.Services;

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
            (Action)(() => ValidationServices.ValidateAndParseAttractionType(input!))
        );

        StringAssert.Contains(ex.Message, "cannot be null or empty");
    }
    #endregion
}
