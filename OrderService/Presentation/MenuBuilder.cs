using OrderService.Builders;

namespace OrderService.Presentation;

public static class MenuBuilder
{
    public static void Run(IBuilder builder)
    {
        string[] inputs = new string[builder.Prompts.Count];
        
        for (int i = 0; i < builder.Prompts.Count; i++)
        {
            Console.WriteLine(builder.Prompts[i]);
            inputs[i] = Console.ReadLine()!;
        }
        
        builder.SubmitInputs(inputs);
    }
}