using OrderService.Repositories;

namespace OrderService.Presentation.MenuSections;

public class DataFormatSelectionMenu(IReadOnlyList<DataFormatType> dataFormats)
{
    public string Title => "Load from:";
    
}