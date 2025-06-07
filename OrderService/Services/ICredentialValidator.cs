using OrderService.Models.Users;

namespace OrderService.Services;

interface ICredentialValidator
{
    RegistrationReport Validate(string username, string passwprd, List<UserCredentials> users, out string message);
}