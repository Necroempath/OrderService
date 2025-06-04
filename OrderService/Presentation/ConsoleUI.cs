using OrderService.Models.Users;
using OrderService.Repositories;

namespace OrderService.Presentation;

public static class ConsoleUI
{
    public enum LogIn
    {
        Registration,
        Authentication
    }
    
    public static string MainMenu(string username)
    {
        Console.WriteLine($"Profile: {username}\n");

        Console.Write("""
                            1. View orders
                            2. Add new order
                            3. Save orders
                            4. Load orders
                            5. Sign out
                          """);
        
        return Console.ReadLine();
    }

    public static IDataRepository<UserProfile> SaveFormatSelection()
    {
        Console.Write("""
                            1. Json file
                            2. Xml file
                            3. Binary file
                            4. Cancel
                            Choose save format: 
                          """);
        
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                return new JsonDataRepository<UserProfile>(string.Empty);
            case "2":
                return new XmlDataRepository<UserProfile>(string.Empty);
            case "3":
                return new JsonDataRepository<UserProfile>(string.Empty);
            default:
                return null;
        }
    }
    
    public static string LogInMenu()
    {
        Console.Write("""
                      1. Sign up
                      2. Sign in
                      3. Exit
                      
                      Choose option by corresponding digit: 
                      """);
        
        return Console.ReadLine();
    }

    public static (string, string) CredentialsInput(LogIn logIn)
    {
      Console.WriteLine($"\n____{logIn}_____\n");

      Console.Write("Enter username: ");
      string username = Console.ReadLine()!;
      
      Console.Write("Enter password: ");
      string password = Console.ReadLine()!;
      
      return (username, password)!;
    }
    
    
    public static void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey(true);
        Console.Clear();
    }
}