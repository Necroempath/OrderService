using OrderService.Models.Users;
using OrderService.Repositories;

namespace OrderService.Models.User;

public class UserDataFormatInfo
{
    private Queue<DataFormatType> _userProfiles = new();

    public UserDataFormatInfo()
    {
        IReadOnlyCollection<DataFormatType> userProfiles = new Queue<DataFormatType>();
    }
    public string Username { get; init; }
    
    public IReadOnlyCollection<DataFormatType> DataFormats 
    {
        get => _userProfiles;
        set => _userProfiles = value as Queue<DataFormatType> ?? new();
    }

    public void AddDataFormat(DataFormatType dataFormat)
    {
        var userProfiles = new Queue<DataFormatType>();
        userProfiles.Enqueue(dataFormat);
        
        foreach (var userProfile in _userProfiles)
        {
            if(userProfile == dataFormat) continue;
            
            userProfiles.Enqueue(userProfile);
        }
        
        _userProfiles = userProfiles;
    }
}