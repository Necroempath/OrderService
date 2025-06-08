using OrderService.Models.Orders;
using OrderService.Repositories;

namespace OrderService.Presentation;

public static class ConsoleUI
{
    public enum LogIn
    {
        Registration,
        Authentication
    }
    
   
    public static string LoadFormatSelection(List<DataFormatType> dataFormatTypes)
    {
        Console.Clear();
        Console.WriteLine("|————— LOAD FROM —————|\n");
        
        for (int i = 0; i < dataFormatTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dataFormatTypes[i]}");
        }
        
        Console.Write("\nSelect the option by the appropriate number: ");

        return Console.ReadLine()!;
    }
  
    public static (string, string) CredentialsInput(LogIn logIn)
    {
      Console.WriteLine($"\n____{logIn}_____\n");

      Console.Write("Enter username: ");
      string username = Console.ReadLine()!;
      
      Console.Write("Enter password: ");

      return (username, Console.ReadLine()!);
    }

    public static void ShowOrders(IReadOnlyList<Order> orders)
    {
        Console.Clear();
        
        if (orders.Count == 0)
        {
            Console.WriteLine("No orders yet.");
        }

        for (int i = 0; i < orders.Count; i++)
        {
            Console.WriteLine(orders[i].Print(i)); 
        }
        
        Pause();
    }
    
    public static void Pause()
    {
        Console.Write("\nPress any key to continue... ");
        Console.ReadKey(true);
        Console.Clear();
    }
}