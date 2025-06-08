using Microsoft.Extensions.Configuration;
using OrderService.Models.Orders;
using OrderService.Models.Services;

using OrderService.Models.Users;
using OrderService.Presentation;
using OrderService.Repositories;
using OrderService.Services;
using OrderService.Builders;
using OrderService.Logging;
using OrderService.Presentation.MenuSections;

namespace OrderService;

class App
{
    private readonly PathProvider _paths;
    private readonly AuthService _authService;
    private UserProfile? _loggedInUserProfile;
    private UserDataFormatInfo? _loggedInUserDataFormatInfo;
    private JsonDataRepository<UserDataFormatInfo> _metadataRepository;
    private ILogger _logger;
    
    public App()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        _paths = new PathProvider(config);
        if (!File.Exists(_paths.GetCredentialsFile()))
        {
            File.WriteAllText(_paths.GetCredentialsFile()!, "[]");
        }
        _logger = new FileLogger();
        _authService = new AuthService(new JsonDataRepository<List<UserCredentials>>("usersCredentials.json"), new DefaultCredentialValidator(), _logger);
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

        while (true)
        {
            switch (MenuManager.Manage(MainMenu.Options, MainMenu.Title))
            {
                case MainMenu.OptionType.ShowOrders:
                    ConsoleUI.ShowOrders(_loggedInUserProfile!.Orders);
                    break;
                
                case MainMenu.OptionType.AddOrder:
                    if (BuildOrder() is { } order)
                    {
                        _loggedInUserProfile!.AddOrder(order);
                        _logger.LogInfo($"New order has been placed by '{_loggedInUserProfile!.Username}'");
                    }
                    break;
                
                case MainMenu.OptionType.ClearOrders:
                    _loggedInUserProfile.ClearOrder();
                    _logger.LogInfo($"Order list of user '{_loggedInUserProfile!.Username}' has been cleared");
                    Console.WriteLine("Order list has been cleared");
                    ConsoleUI.Pause();
                    break;
                
                case MainMenu.OptionType.SaveData:
                {
                    var formatTypeNullable = MenuManager.Manage(FileSaveMenu.Options, FileSaveMenu.Title);
                    if (formatTypeNullable is not { } formatType)
                    {
                        Console.WriteLine("Invalid input");
                        ConsoleUI.Pause();
                        break;
                    }
                    
                    SaveUserProfile(formatType);
                    
                    _loggedInUserDataFormatInfo.AddFormatType(formatType);
                    _metadataRepository.SaveData(_loggedInUserDataFormatInfo);
                    
                    break;
                }
                
                case MainMenu.OptionType.LoadData:
                {
                    var availableDataFiles = _metadataRepository.LoadData()!.DataFormatTypes;
                    
                    if (availableDataFiles.Count == 0)
                    {
                        Console.WriteLine("\nNo data found");
                        ConsoleUI.Pause();
                        
                        break;
                    }
                    
                    var dataLoadFormat = InputHandler.Handle(_metadataRepository.LoadData()!.DataFormatTypes, ConsoleUI.LoadFormatSelection);
                    
                    if (dataLoadFormat is not { } loadType)
                    {
                        Console.WriteLine("Invalid input");
                        ConsoleUI.Pause();
                        break;
                    }
                    _loggedInUserProfile = LoadUserProfile(loadType);
                    
                    break;
                }
                
                case MainMenu.OptionType.SignOut:
                    _loggedInUserProfile = null;
                    _logger.LogInfo($"User {_loggedInUserProfile!.Username} signed out.");
                    
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
        IDataRepository<UserProfile> profileRepository = null;
        switch (type)
        {
            case DataFormatType.JsonFile:
                profileRepository = new JsonDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, type)!);
                break;
            case DataFormatType.XmlFile:
                profileRepository = new XmlDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, type)!);
                break;
            case DataFormatType.BinaryFile:
                profileRepository = new BinaryDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, type)!);
                break;
        }

        try
        {
            profileRepository!.SaveData(_loggedInUserProfile!);
        }
        catch (Exception e)
        {
            _logger.LogError($"User '{_loggedInUserProfile!.Username}' failed to save data in {type}.'");
            Console.WriteLine(e);
        }
        
        _logger.LogInfo($"Data of user '{_loggedInUserProfile!.Username}' has been successfully saved in {type}.");

        Console.WriteLine($"Data of user '{_loggedInUserProfile!.Username}' has been successfully saved in {type}.");
        ConsoleUI.Pause();
    }

    private UserProfile LoadUserProfile(DataFormatType type)
    {
        IDataRepository<UserProfile> profileRepository = null;
        
        switch (type)
        {
            case DataFormatType.JsonFile:
                profileRepository = new JsonDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, DataFormatType.JsonFile)!);
                break;
            case DataFormatType.XmlFile:
                profileRepository = new XmlDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, DataFormatType.XmlFile)!);
                break;
            case DataFormatType.BinaryFile:
                profileRepository = new BinaryDataRepository<UserProfile>(_paths.GetProfileDataFile(_loggedInUserProfile!.Username, DataFormatType.BinaryFile)!);
                break;
        }
        
        UserProfile? profile = null;
        try
        {
            profile = profileRepository!.LoadData()!;
        }
        catch (Exception e)
        {
            _logger.LogError($"User '{_loggedInUserProfile!.Username}' failed to load data from {type}.'");
            Console.WriteLine(e);
        }
        
        _logger.LogInfo($"Data of user '{_loggedInUserProfile!.Username}' has been successfully loaded from {type}.");

        Console.WriteLine($"Data of user '{_loggedInUserProfile!.Username}' has been successfully loaded from {type}.");
        ConsoleUI.Pause();
        
        return profile;
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
                Console.Write("Invalid option.");
                ConsoleUI.Pause();
                return false;
        }
        
        if(_loggedInUserProfile is null) return false;
        
        _metadataRepository =  new JsonDataRepository<UserDataFormatInfo>(_paths.GetFormatInfoFile(_loggedInUserProfile!.Username)!);
        _loggedInUserDataFormatInfo = _metadataRepository.LoadData()!;
        
        if (_loggedInUserDataFormatInfo.DataFormatTypes.Count > 0)
        {
            _loggedInUserProfile = LoadUserProfile(_loggedInUserDataFormatInfo.DataFormatTypes[0]);
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