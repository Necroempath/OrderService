using OrderService.Repositories;

namespace OrderService.Presentation.MenuSections;

public static class FileSaveMenu
{
    public static string Title => "|————— SAVE AS —————|";
    
    public static IReadOnlyList<(DataFormatType, string)> Options =>
    [
        (DataFormatType.JsonFile, "Json file"),
        (DataFormatType.XmlFile, "Xml file"),
        (DataFormatType.BinaryFile, "Binary file")
    ];
}
