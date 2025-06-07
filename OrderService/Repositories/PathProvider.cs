using Microsoft.Extensions.Configuration;

namespace OrderService.Repositories;

public class PathProvider(IConfiguration configuration)
{
    public string? GetCredentialsFile()
    {
        return configuration["Paths:UsersCredentialsFile"];
    }

    public string? GetFormatInfoFile(string username)
    {
        string? path = configuration["Paths:UserDataFormatInfoFile"];
        path = path?.Replace("{username}", username);
        
        return path;
    }
    
    public string? GetProfileDataFolder(string username)
    {
        string? path = configuration["Paths:UserProfileDataFolder"];
        path = path?.Replace("{username}", username);
        
        return path;
    }
    
    public string? GetProfileDataFile(string username, DataFormatType dataFormat)
    {
        var fileName = string.Empty;
        
        switch (dataFormat)
        {
            case DataFormatType.JsonFormatData:
                fileName = username + ".json";
                break;
            case DataFormatType.XmlFormatData:
                fileName = username + ".xml";
                break;
            case DataFormatType.BinaryFormatData:
                fileName = username + ".dat";
                break;
        }
        string path = configuration["Paths:UserProfileDataFolder"]!;
        path = path.Replace("{username}", username);
        path = Path.Combine(path, fileName);
        
        return path;
    }
    
    public string? GetFormatInfoFolder(string username)
    {
        string? path = configuration["Paths:UserDataFormatInfoFolder"];
        path = path?.Replace("{username}", username);
        
        return path;
    }
}