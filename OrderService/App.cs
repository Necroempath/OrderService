using OrderService.Models.User;
using OrderService.Models.Users;
using OrderService.Presentation;
using OrderService.Repositories;
using OrderService.Services;

namespace OrderService;

class App
{
    private AuthService _authService;
    private UserProfile? _loggedInUser = null;
    private bool _newUserRegistered = false;
    public App()
    {
        _authService = new AuthService(new JsonDataRepository<List<UserCredentials>>("usersCredentials.json"), new DefaultCredentialValidator());
    }

    public void Run()
    {
        while (_authService.EmptyUsersCredentials())
        {
            _loggedInUser = Registration();
        }

        while (!_authService.LoggedIn)
        {
            switch (ConsoleUI.LogInMenu())
            {
                case "1":
                    _loggedInUser = Registration();
                    break;
                case "2":
                    _loggedInUser = Authentication();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Incorrect menu option selection.");
                    ConsoleUI.Pause();
                    break;
            }
        }

        if (_newUserRegistered)
        {
            CreateUserProfileRepository(_loggedInUser!);
        }
        
       switch(ConsoleUI.MainMenu(_loggedInUser!.Username))
        {
            case "1":
                ViewOrders();
                break;
            case "2":
                AddNewOrder();
                break;
            case "3":
                IDataRepository <UserProfile> saveFormat = ConsoleUI.SaveFormatSelection();
                saveFormat.SaveData(_loggedInUser!);
                Console.WriteLine($"Data of user {_loggedInUser!.Username} has been successfully saved.");
                break;
            case "4":
                
        }
    }

    private void CreateUserProfileRepository(UserProfile loggedInUser)
    {
        string folderPath = "data/" + $"{loggedInUser.Username}/";
        Directory.CreateDirectory(folderPath);
    }

    private UserProfile? Registration()
    { 
      (string username, string password) = ConsoleUI.CredentialsInput(ConsoleUI.LogIn.Registration);
      
      var userProfile = _authService.Register(username, password, out RegistrationReport report, out string message);
      
      Console.WriteLine(message);
      ConsoleUI.Pause();
      
      _newUserRegistered = true;
      
      return userProfile;
    }

    private UserProfile? Authentication()
    {
        (string username, string password) = ConsoleUI.CredentialsInput(ConsoleUI.LogIn.Authentication);

        var userProfile = _authService.Authenticate(username, password, out AuthenticatioReport report, out string message);
            
        Console.WriteLine(message);
        ConsoleUI.Pause();
        
        return userProfile;
    }
}