using System.Text;

namespace OrderService.Presentation.MenuSections;

public static class MainMenu
{
    public enum OptionType
    {
        ShowOrders,
        AddOrder,
        ClearOrders,
        SaveData,
        LoadData,
        SignOut
    }

    public static string Title => "|————— MAIN MENU —————|";

    public static IReadOnlyList<(OptionType, string)> Options =>
    [
        (OptionType.ShowOrders, "Show Orders"),
        (OptionType.AddOrder, "Add Order"),
        (OptionType.ClearOrders, "Clear Order list"),
        (OptionType.SaveData, "Save Data"),
        (OptionType.LoadData, "Load Data"),
        (OptionType.SignOut, "Sign out")
    ];
}