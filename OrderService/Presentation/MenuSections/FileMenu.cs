using OrderService.Repositories;

namespace OrderService.Presentation.MenuSections;

public static class FileSaveMenu
{
    public static string Title => "|————— SAVE AS —————|";
    
    public static IReadOnlyList<(DataFormatType, string)> Options =>
    [
        (DataFormatType.JsonFormatData, "Json file"),
        (DataFormatType.BinaryFormatData, "Xml file"),
        (DataFormatType.BinaryFormatData, "Binary file")
    ];
}
