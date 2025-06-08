namespace OrderService.Presentation;

public class InputHandler
{
    public static TResult? Handle<TResult>(List<TResult> options, Func<List<TResult>, string> output) where TResult : struct, Enum
    {
        string input = output(options);
        
        if (int.TryParse(input, out int result))
        {
            if (result > 0 || result <= options.Count)
            {
                return options[result - 1];
            }
        }

        return null;
    }
}