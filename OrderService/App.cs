using Microsoft.Extensions.Configuration;
using OrderService.Models.Orders;
using OrderService.Models.Services;

using OrderService.Models.Users;
using OrderService.Presentation;
using OrderService.Repositories;
using OrderService.Services;
using OrderService.Builders;
using OrderService.Presentation.MenuSections;

namespace OrderService;

class App
{
    private readonly PathProvider _paths;
    private readonly AuthService _authService;
    private UserProfile? _loggedInUserProfile;
    private UserDataFormatInfo? _loggedInUserDataFormatInfo;
    
    public App()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _paths = new PathProvider(config);
        _authService = new AuthService(new JsonDataRepository<List<UserCredentials>>("usersCredentials.json"), new DefaultCredentialValidator());
    }

    public void Run()
    {
        while (_authService.EmptyUsersCredentials())
        {
            _loggedInUserProfile = Registration();
        }

        while (_loggedInUserProfile is null)
        {
            if (Login()) return;
        }

        var metadataRepository = new JsonDataRepository<UserDataFormatInfo>(_paths.GetFormatInfoFile(_loggedInUserProfile.Username)!);
        _loggedInUserDataFormatInfo = metadataRepository.LoadData()!;
        if (_loggedInUserDataFormatInfo.DataFormatTypes.Count > 0)
        {
            _loggedInUserProfile = LoadUserProfile(_loggedInUserDataFormatInfo.DataFormatTypes.Peek());
        }

        while (true)
        {
            switch (MenuManager.Manage(MainMenu.Options, MainMenu.Title))
            {
                case MainMenu.OptionType.ShowOrders:
                    ConsoleUI.ShowOrders(_loggedInUserProfile!.Orders);
                    break;
                
                case MainMenu.OptionType.AddOrder:
                    if(BuildOrder() is { } order) _loggedInUserProfile!.AddOrder(order);
                    break;
                
                case MainMenu.OptionType.SaveData:
                {
                    var formatTypeNullable = MenuManager.Manage(FileSaveMenu.Options, FileSaveMenu.Title);
                    if (formatTypeNullable is not { } formatType)
                    {
                        Console.WriteLine("Invalid input");
                        break;
                    }
                    
                    SaveUserProfile(formatType);
                    
                    _loggedInUserDataFormatInfo.AddFormatType(formatType);
                    metadataRepository.SaveData(_loggedInUserDataFormatInfo);
                    Console.WriteLine($"Data of user {_loggedInUserProfile!.Username} has been successfully saved.");
                    break;
                }
                case MainMenu.OptionType.LoadData:
                {
                   // IDataRepository<UserDataFormatInfo> metadataRepository = new JsonDataRepository<UserDataFormatInfo>($"user_metadata/{_loggedInUserProfile.Username}/{_loggedInUserProfile.Username}.json");
                    var dataLoadFormat = ConsoleUI.LoadFormatSelection(metadataRepository.LoadData().DataFormatTypes.ToList());
                    IDataRepository<UserProfile> profileRepository = null;
                    switch (dataLoadFormat)
                    {
                        case DataFormatType.JsonFormatData:
                            profileRepository =
                                new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUserProfile.Username}/{_loggedInUserProfile.Username}.json");
                            break;
                        case DataFormatType.XmlFormatData:
                            profileRepository =
                                new XmlDataRepository<UserProfile>($"user_profile_data/{_loggedInUserProfile.Username}/{_loggedInUserProfile.Username}.xml");
                            break;
                        case DataFormatType.BinaryFormatData:
                            profileRepository =
                                new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUserProfile.Username}/{_loggedInUserProfile.Username}.json");
                            break;
                    }

                    _loggedInUserProfile = profileRepository?.LoadData();
                    break;
                }
                case MainMenu.OptionType.SignOut:
                    _loggedInUserProfile = null;
                    
                    while (_loggedInUserProfile is null)
                    {
                        if (Login()) return;
                    }
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    ConsoleUI.Pause();
                    break;
            }
        }
    }

    private void SaveUserProfile(DataFormatType type)
    {
        switch (type)
        {
            case DataFormatType.JsonFormatData:
                new JsonDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, type)!).SaveData(_loggedInUserProfile!);
                break;
            case DataFormatType.XmlFormatData:
                new XmlDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, type)!).SaveData(_loggedInUserProfile!);
                break;
            case DataFormatType.BinaryFormatData:
                break;
        }
    }

    private UserProfile LoadUserProfile(DataFormatType type)
    {
        IDataRepository<UserProfile> profileRepository = null;
        
        switch (type)
        {
            case DataFormatType.JsonFormatData:
                profileRepository = new JsonDataRepository<UserProfile>(_paths.GetProfileDataFolder(_loggedInUserProfile!.Username)!);
                break;
            case DataFormatType.XmlFormatData:
                profileRepository = new XmlDataRepository<UserProfile>(_paths.GetProfileDataFolder(_loggedInUserProfile!.Username)!);
                break;
            // case DataFormatType.BinaryFormatData:
            //     break;
        }
        
        return profileRepository.LoadData()!;
    }

    private bool Login()
    {
        switch (MenuManager.Manage(LoginMenu.Options, LoginMenu.Title))
        {
            case LoginMenu.OptionType.SignIn:
                _loggedInUserProfile = Authorization();
                break;
            case LoginMenu.OptionType.SignUp:
                _loggedInUserProfile = Registration();
                break;
            case LoginMenu.OptionType.Exit:
                return true;
            default:
                Console.Write("Invalid option. ");
                ConsoleUI.Pause();
                break;
        }

        return false;
    }

    private UserProfile? Registration()
    { 
      (string username, string password) = ConsoleUI.CredentialsInput(ConsoleUI.LogIn.Registration);
      
      var userProfile = _authService.Register(username, password, out RegistrationReport report, out string message);
      
      Console.WriteLine(message);
      ConsoleUI.Pause();
      
      if(userProfile == null) return null;
      
      Directory.CreateDirectory(_paths.GetProfileDataFolder(username)!); 
      Directory.CreateDirectory(_paths.GetFormatInfoFolder(username)!); 
      new JsonDataRepository<UserDataFormatInfo>(_paths.GetFormatInfoFile(username)!).SaveData(new UserDataFormatInfo { Username = username });
      
      return userProfile;
    }

    private UserProfile? Authorization()
    {
        (string username, string password) = ConsoleUI.CredentialsInput(ConsoleUI.LogIn.Authentication);

        var userProfile = _authService.Authenticate(username, password, out AuthenticatioReport report, out string message);
            
        Console.WriteLine($"\n{message}");
        ConsoleUI.Pause();
        
        return userProfile;
    }

    private static Service? BuildService()
    {
        ServiceBuilder serviceBuilder = new();
        BuilderMenu.Run(serviceBuilder);
        var report = serviceBuilder.Validate(out string message);

        Console.WriteLine(message);
        if(report != ServiceBuilder.BuildingReport.ValidData) ConsoleUI.Pause();
        
        return serviceBuilder.Build();
    }

    private static Order? BuildOrder()
    {
        OrderBuilder orderBuilder = new();
        
        do
        {
            Console.Clear();
            if (BuildService() is not Service service) return null;

            orderBuilder.AddService(service);
            
            Console.WriteLine("New service has been included. \n1. Add another one\n2. Continue");

        } while (Console.ReadLine() == "1");
        
        BuilderMenu.Run(orderBuilder);

        orderBuilder.Validate(out string message);
        
        Console.WriteLine(message);
        ConsoleUI.Pause();
            
        return orderBuilder.Build();
    }
}