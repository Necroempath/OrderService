using OrderService.Builders;

namespace OrderService.Presentation;

public static class BuilderMenu
{
    public static void Run(IBuilder builder)
    {
        string[] inputs = new string[builder.Prompts.Count];
        
        for (int i = 0; i < builder.Prompts.Count; i++)
        {
            Console.Write(builder.Prompts[i]);
            inputs[i] = Console.ReadLine()!;
        }
        
        builder.SubmitInputs(inputs);
    }
}