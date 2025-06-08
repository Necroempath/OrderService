using OrderService.Repositories;

namespace OrderService.Models.Users;

public class UserDataFormatInfo
{
    public required string Username { get; init; }

    public List<DataFormatType> DataFormatTypes { get; set; } = new();

    public void AddFormatType(DataFormatType formatType)
    {
        List<DataFormatType> list = new();
        list.Add(formatType);

        list.AddRange(DataFormatTypes.Where(type => formatType != type));

        DataFormatTypes = list;
    }
}