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
    private UserProfile? _loggedInUser;
    
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
            _loggedInUser = Registration();
        }

        while (_loggedInUser is null)
        {
            if (Login()) return;
        }
        
        while (true)
        {
            switch (MenuManager.Manage(MainMenu.Options, MainMenu.Title))
            {
                case MainMenu.OptionType.ShowOrders:
                    ConsoleUI.ShowOrders(_loggedInUser!.Orders);
                    break;
                
                case MainMenu.OptionType.AddOrder:
                    if(BuildOrder() is { } order) _loggedInUser!.AddOrder(order);
                    break;
                
                case MainMenu.OptionType.SaveData:
                {
                    IDataRepository<UserProfile> profileRepository = null;
                    IDataRepository <UserDataFormatInfo> metadataRepository = new JsonDataRepository<UserDataFormatInfo>($"user_metadata/{_loggedInUser.Username}/{_loggedInUser.Username}.json");
                    UserDataFormatInfo userDataFormatInfo = metadataRepository.LoadData()!;
                    DataFormatType formatType = DataFormatType.JsonFormatData;
                    
                    switch (string.Empty)
                    {

                        case "1":
                            profileRepository = new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.json");
                            formatType = DataFormatType.JsonFormatData;
                            break;
                        case "2":
                            profileRepository =
                                new XmlDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.xml");
                            formatType = DataFormatType.XmlFormatData;
                            break;
                        case "3":
                            profileRepository =
                                new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.dat");
                            formatType = DataFormatType.BinaryFormatData;
                            break;
                    }
                    
                    userDataFormatInfo.AddFormatType(formatType);
                    metadataRepository.SaveData(userDataFormatInfo);
                    profileRepository?.SaveData(_loggedInUser!);
                    Console.WriteLine($"Data of user {_loggedInUser!.Username} has been successfully saved.");
                    break;
                }
                case MainMenu.OptionType.LoadData:
                {
                    IDataRepository<UserDataFormatInfo> metadataRepository = new JsonDataRepository<UserDataFormatInfo>($"user_metadata/{_loggedInUser.Username}/{_loggedInUser.Username}.json");
                    var dataLoadFormat = ConsoleUI.LoadFormatSelection(metadataRepository.LoadData().DataFormatTypes.ToList());
                    IDataRepository<UserProfile> profileRepository = null;
                    switch (dataLoadFormat)
                    {
                        case DataFormatType.JsonFormatData:
                            profileRepository =
                                new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.json");
                            break;
                        case DataFormatType.XmlFormatData:
                            profileRepository =
                                new XmlDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.xml");
                            break;
                        case DataFormatType.BinaryFormatData:
                            profileRepository =
                                new JsonDataRepository<UserProfile>($"user_profile_data/{_loggedInUser.Username}/{_loggedInUser.Username}.json");
                            break;
                    }

                    _loggedInUser = profileRepository?.LoadData();
                    break;
                }
                case MainMenu.OptionType.SignOut:
                    if (Login()) return;
                    break;
            }
        }
    }

    private bool Login()
    {
        switch (MenuManager.Manage(LoginMenu.Options, LoginMenu.Title))
        {
            case LoginMenu.OptionType.SignIn:
                _loggedInUser = Authorization();
                break;
            case LoginMenu.OptionType.SignUp:
                _loggedInUser = Registration();
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
      
      Directory.CreateDirectory(_paths.GetFormatInfo(_loggedInUser!.Username)!);
      new JsonDataRepository<UserDataFormatInfo>(_paths.GetFormatInfo(_loggedInUser!.Username)!).SaveData(new UserDataFormatInfo { Username = _loggedInUser!.Username });
      
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
        MenuBuilder.Run(serviceBuilder);
        serviceBuilder.Validate(out string message);

        Console.WriteLine(message);
        
        return serviceBuilder.Build();
    }

    private static Order? BuildOrder()
    {
        OrderBuilder orderBuilder = new();
        
        do
        {
            if (BuildService() is not Service service) return null;

            orderBuilder.AddService(service);

            orderBuilder.AddService(service);
            Console.WriteLine("New service has been included. \n1. Add another one\n2. Continue");

        } while (Console.ReadLine() == "1");
        
        MenuBuilder.Run(orderBuilder);

        orderBuilder.Validate(out string message);
        
        Console.WriteLine(message);
        ConsoleUI.Pause();
            
        return orderBuilder.Build();
    }
}