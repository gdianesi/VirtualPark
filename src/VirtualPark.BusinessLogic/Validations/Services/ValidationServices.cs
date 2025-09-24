namespace VirtualPark.BusinessLogic.Validations.Services;

public static class ValidationServices
{
    public static int ValidateAndParseInt(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        try
        {
            return int.Parse(number);
        }
        catch (FormatException)
        {
            throw new FormatException($"The value '{number}' is not a valid integer.");
        }
    }
}
