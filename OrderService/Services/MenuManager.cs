using System.Collections;
using System.Globalization;
using OrderService.Presentation.MenuSections;
namespace OrderService.Services;

public class MenuManager
{
    public static TResult? Manage<TResult>(IReadOnlyList<(TResult, string)> options, string title) where TResult : struct, Enum
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine();

        for(int i = 0; i < options.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {options[i].Item2}");
        }

        Console.Write("\nSelect the option by the appropriate number: ");
        
        if (int.TryParse(Console.ReadLine(), out int option))
        {
            if (option > 0 && option <= options.Count)
            {
                return options[option - 1].Item1;
            }
        }
        
        return null;
    }
}