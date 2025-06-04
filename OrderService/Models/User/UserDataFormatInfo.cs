using OrderService.Models.Users;
using OrderService.Repositories;

namespace OrderService.Models.User;

public class UserDataFormatInfo
{
    private Stack<DataFormatType> _userProfiles = new();

    public UserDataFormatInfo()
    {
        IReadOnlyCollection<DataFormatType> userProfiles = new Stack<DataFormatType>();
    }
    public string Username { get; init; }
    
    public IReadOnlyCollection<DataFormatType> DataFormats 
    {
        get => _userProfiles;
        set => _userProfiles = value as Stack<DataFormatType> ?? new();
    }

    public void AddDataFormat(DataFormatType dataFormat)
    {
        var userProfiles = new Stack<DataFormatType>();

        foreach (var userProfile in _userProfiles)
        {
            if(userProfile == dataFormat) continue;
            
            userProfiles.Push(userProfile);
        }
        
        userProfiles.Push(dataFormat);
        _userProfiles = userProfiles;
    }
}