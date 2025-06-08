using OrderService.Logging;
using OrderService.Models.Users;
using OrderService.Repositories;

namespace OrderService.Services;

class AuthService(IDataRepository<List<UserCredentials>> repository, ICredentialValidator credentialValidator, ILogger logger)
{
    private readonly List<UserCredentials> _usersCredentials = repository.LoadData()!;
    

    public UserProfile? Register(string username, string password, out RegistrationReport report, out string message)
    {
        report = credentialValidator.Validate(username, password, _usersCredentials, out message);
        
        if (report == RegistrationReport.ValidCredentials)
        {
            string salt = Hasher.GenerateSalt();
            string hashedPassword = Hasher.HashPassword(password, salt);
            
            _usersCredentials.Add(new UserCredentials { Username = username, HashPassword = hashedPassword, Salt = salt });
            repository.SaveData(_usersCredentials);
            
            logger.LogInfo($"New user '{username}' has been registered");
            return new UserProfile { Username = username };
        }
        
        logger.LogError("Failed to register new user");
        return null;
    }

    public UserProfile? Authenticate(string username, string password, out AuthenticatioReport report, out string message)
    {
        var existed = _usersCredentials.Find(u => u.Username == username);

        if (existed != null)
        {
            string hashPassword = Hasher.HashPassword(password, existed.Salt);
            
            if (existed.HashPassword == hashPassword)
            {
                message = $"User '{username}' logged in successfully";
                report = AuthenticatioReport.UserFound;

                logger.LogInfo(message);
                return new UserProfile { Username = username };
            }
            
            message = $"Password '{password}' does not match to given username '{username}'";
            report = AuthenticatioReport.PasswordDoesntMatch;

            logger.LogError("Incorrect password");
            return null;
        }

        message = "User not found";
        logger.LogError("Failed to authenticate user");
        report = AuthenticatioReport.IncorrectUsername;
        
        return null;
    }

    public bool EmptyUsersCredentials()
    {
        return _usersCredentials.Count == 0;
    }
}
