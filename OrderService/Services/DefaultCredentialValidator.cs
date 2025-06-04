using OrderService.Models.User;

namespace OrderService.Services;

class DefaultCredentialValidator : ICredentialValidator
{
    public int UsernameMinLen => 5;
    public int UsernameMaxLen => 20;
    public int PasswordMinLen => 6;
    public int PasswordMaxLen => 20;
    public string Message { get; private set; }

    public RegistrationReport Validate(string username, string password,  List<UserCredentials> users, out string message)
    {

        if (users.Find(u => u.Username == username) != null) 
        { 
            message = $"Username '{username}' is already taken";
            return RegistrationReport.InvalidUsername;
        }

        if (username.Length < UsernameMinLen)
        {
            message = $"Username '{username}' is too short. Lenght must be at least {UsernameMinLen} characters";
            return RegistrationReport.InvalidUsername;
        }

        if (username.Length > UsernameMaxLen)
        {
            message = $"Username '{username}' is too long. Lenght must not be more than {UsernameMaxLen} characters";
            return RegistrationReport.InvalidUsername;
        }

        if (password.Length < PasswordMinLen)
        {
            message = $"Password '{password}' is too short. Lenght must be at least {PasswordMinLen} characters";
            return RegistrationReport.InvalidPassword;
        }
        
        if (password.Length > PasswordMaxLen)
        {
            message = $"Password '{password}' is too long. Lenght must not be more than {PasswordMaxLen} characters";
            return RegistrationReport.InvalidPassword;
        }
        
        message = $"Username '{username}' has been successfully created";
        return RegistrationReport.ValidCredentials;
    }
}