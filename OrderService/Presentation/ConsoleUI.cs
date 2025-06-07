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
    
   
    public static DataFormatType? LoadFormatSelection(List<DataFormatType> dataFormatTypes)
    {
        if (dataFormatTypes.Count == 0)
        {
            Console.WriteLine("No data has been saved for the current user yet.");
            return null;
        }

        for (int i = 0; i < dataFormatTypes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {dataFormatTypes[i]}");
        }

        Console.Write("Choose option by corresponding digit: ");

        if (int.TryParse(Console.ReadLine(), out int option))
        {
            if (option > 0 && option <= dataFormatTypes.Count)
            {
                return dataFormatTypes[option - 1];
            }

            Console.WriteLine("Invalid option.");
            return null;
        }

        Console.WriteLine("Incorrect input.");
        return null;
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