using FluentAssertions;
using VirtualPark.BusinessLogic.Validations.Services;

namespace VirtualPark.BusinessLogic.Test.Validations;

[TestClass]
[TestCategory("Validations")]
public class ValidationTest
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
}
