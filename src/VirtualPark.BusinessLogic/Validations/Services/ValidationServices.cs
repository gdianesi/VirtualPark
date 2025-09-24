namespace VirtualPark.BusinessLogic.Validations.Services;

public static class ValidationServices
{
    public static int ValidateAndParseInt(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        if (!int.TryParse(number, out var result))
        {
            throw new FormatException($"The value '{number}' is not a valid integer.");
        }

        return result;
    }

    public static bool ValidateAndParseBool(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        if (!bool.TryParse(value, out var result))
        {
            throw new FormatException($"The value '{value}' is not a valid boolean.");
        }

        return result;
    }
}
