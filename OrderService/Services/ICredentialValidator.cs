using OrderService.Models.User;

namespace OrderService.Services;

interface ICredentialValidator
{
    RegistrationReport Validate(string username, string passwprd, List<UserCredentials> users, out string message);
}