using System.Net.Mail;
using System.Text.RegularExpressions;
using VirtualPark.BusinessLogic.VisitorsProfile.Models;

namespace VirtualPark.BusinessLogic.Users.Models;

public class UserArgs(string name, string lastName, string email, string password)
{
    public string Name { get; init; } = name;
    public string LastName { get; init; } = lastName;
    public string Email { get; init; } = ValidateEmail(email);
    public string Password { get; init; } = ValidatePassword(password);
    public VisitorProfileArgs? VisitorProfile { get; set; }

    private static string ValidateEmail(string email)
    {
        try
        {
            var ok = new MailAddress(email);
            return email;
        }
        catch
        {
            throw new ArgumentException($"Invalid email format: {email}", nameof(email));
        }
    }

    private static string ValidatePassword(string password)
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
}
