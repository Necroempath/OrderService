using Microsoft.Extensions.Configuration;

namespace OrderService.Repositories;

public class PathProvider(IConfiguration configuration)
{
    public string? GetCredentialsPath()
    {
        return configuration["Paths:UsersCredentialsFile"];
    }

    public string? GetProfileData(string username)
    {
        string? path = configuration["Paths:UsersProfileDataFile"];
        path = path?.Replace("{username}", username);
        
        return path;
    }

    public string? GetFormatInfo(string username)
    {
        string? path = configuration["Paths:UsersProfileDataFile"];
        path = path?.Replace("{username}", username);
        
        return path;
    }
}
// "UsersCredentialsFile": "usersCredentials.json",
// "UserProfileDataFile": "user_profile_data/{username}/{username}.json",
// "UserDataFormatInfoFile": "user_metadata/{username}/{username}.json"