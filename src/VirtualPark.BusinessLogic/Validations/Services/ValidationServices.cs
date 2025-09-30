using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using VirtualPark.BusinessLogic.Attractions;
using VirtualPark.BusinessLogic.Tickets;
using VirtualPark.BusinessLogic.VisitorsProfile.Entity;

namespace VirtualPark.BusinessLogic.Validations.Services;

public static class ValidationServices
{
    public static int ValidateAndParseInt(string number)
    {
        if(string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        if(!int.TryParse(number, out var result))
        {
            throw new FormatException($"The value '{number}' is not a valid integer.");
        }

        return result;
    }

    public static bool ValidateAndParseBool(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        if(!bool.TryParse(value, out var result))
        {
            throw new FormatException($"The value '{value}' is not a valid boolean.");
        }

        return result;
    }

    public static Guid ValidateAndParseGuid(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        if(!Guid.TryParse(value, out var result))
        {
            throw new FormatException($"The value '{value}' is not a valid GUID.");
        }

        return result;
    }

    public static AttractionType ValidateAndParseAttractionType(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Attraction type cannot be null or empty.");
        }

        if(!Enum.TryParse<AttractionType>(value, true, out var result))
        {
            throw new ArgumentException(
                $"The value '{value}' is not a valid AttractionType. " +
                $"Valid values are: {string.Join(", ", Enum.GetNames(typeof(AttractionType)))}");
        }

        return result;
    }

    public static void ValidateAge(int age)
    {
        if(age <= 0 || age >= 100)
        {
            throw new ArgumentOutOfRangeException(nameof(age), "Age must be between 1 and 99.");
        }
    }

    public static DateOnly ValidateDateOnly(string date)
    {
        if(!DateOnly.TryParseExact(date, "yyyy-MM-dd", out DateOnly parsedDate))
        {
            throw new ArgumentException(
                $"Invalid date format: {date}. Expected format is yyyy-MM-dd");
        }

        if(parsedDate < DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException(
                $"Invalid date: {parsedDate:yyyy-MM-dd}. Date cannot be in the past");
        }

        return parsedDate;
    }

    public static DateTime ValidateDateTime(string date)
    {
        var formats = new[]
        {
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd HH:mm:ss"
        };

        if(!DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
        {
            throw new ArgumentException(
                $"Invalid date format: {date}. Expected format is yyyy-MM-dd or yyyy-MM-dd HH:mm[:ss]");
        }

        return parsedDate;
    }

    public static string ValidateNullOrEmpty(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Value cannot be null or empty.");
        }

        return name;
    }

    public static string ValidateEmail(string email)
    {
        try
        {
            var unused = new MailAddress(email);
            return email;
        }
        catch
        {
            throw new ArgumentException($"Invalid email format: {email}", nameof(email));
        }
    }

    public static string ValidatePassword(string password)
    {
        var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

        var isNotValid = !regex.IsMatch(password);
        if(isNotValid)
        {
            throw new ArgumentException(
                "Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.",
                nameof(password));
        }

        return password;
    }

    public static DateOnly ParseDateOfBirth(string dateOfBirth)
    {
        var isNotValid = !DateOnly.TryParseExact(dateOfBirth, "yyyy-MM-dd", out var parsedDate);
        if(isNotValid)
        {
            throw new ArgumentException(
                $"Invalid date format: {dateOfBirth}. Expected format is yyyy-MM-dd",
                nameof(dateOfBirth));
        }

        return parsedDate;
    }

    public static Membership ParseMembership(string membership)
    {
        var isNotValid = !Enum.TryParse<Membership>(membership, true, out var parsedMembership);
        if(isNotValid)
        {
            throw new ArgumentException(
                $"Invalid membership value: {membership}",
                nameof(membership));
        }

        return parsedMembership;
    }

    public static EntranceType ParseEntranceType(string type)
    {
        var isNotValid = !Enum.TryParse<EntranceType>(type, true, out var parsedType);
        if(isNotValid)
        {
            throw new ArgumentException(
                $"Invalid entrance type value: {type}");
        }

        return parsedType;
    }

    public static List<Guid> ValidateGuidsList(List<Guid> ids)
    {
        if(ids == null || ids.Count == 0)
        {
            throw new ArgumentException("List cannot be null or empty");
        }

        if(ids.Any(id => id == Guid.Empty))
        {
            throw new ArgumentException("List contains invalid Guid");
        }

        return ids;
    }
}
