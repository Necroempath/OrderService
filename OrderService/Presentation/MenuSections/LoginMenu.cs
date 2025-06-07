using System.Text;

namespace OrderService.Presentation.MenuSections;

public static class LoginMenu
{
    public enum OptionType
    {
        SignIn,
        SignUp,
        Exit
    }
    
    public static string Title => "|————— LOG IN —————|";
    
    public static IReadOnlyList<(OptionType, string)> Options =>
    [
        (OptionType.SignIn, "Sign in"),
        (OptionType.SignUp, "Sign up"),
        (OptionType.Exit, "Exit")
    ];
    
    // public static string ToString()
    // {
    //     var builder = new StringBuilder();
    //     
    //     builder.AppendLine(Title);
    //     builder.AppendLine("\n");
    //     
    //     for (int i = 0; i < Options.Count; i++)
    //     {
    //         builder.AppendLine($"{i + 1}. {Options[i].Item2}");
    //     }
    //
    //     return builder.ToString();
    // }
}