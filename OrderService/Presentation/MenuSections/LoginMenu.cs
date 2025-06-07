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
}