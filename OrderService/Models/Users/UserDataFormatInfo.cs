using OrderService.Repositories;

namespace OrderService.Models.Users;

public class UserDataFormatInfo
{

    public required string Username { get; init; }

    public Queue<DataFormatType> DataFormatTypes { get; private set; } =  new();

    public void AddFormatType(DataFormatType formatType)
    {
        Queue<DataFormatType> queue = new();
        queue.Enqueue(formatType);
        
        foreach (var type in DataFormatTypes)
        {
            if(formatType == type) continue;
            queue.Enqueue(type);
        }
        
        DataFormatTypes = queue;
    }
}