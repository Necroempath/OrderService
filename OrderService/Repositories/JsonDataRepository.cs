using System.Text.Json;
using OrderService.Models.User;

namespace OrderService.Repositories;

class JsonDataRepository<T>(string fileName) : IDataRepository<T> where T : class
{
    private readonly JsonSerializerOptions _jsonOptions =  new () { WriteIndented = true };
    
    public T? LoadData()
    {
        using FileStream file = new (fileName, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new (file);
        string fileContent = reader.ReadToEnd();
        
        return JsonSerializer.Deserialize<T>(fileContent);
    }

    public void SaveData(T data)
    {
        using FileStream file = new (fileName, FileMode.Open, FileAccess.Write);
        using StreamWriter writer = new (file);
        writer.Write(JsonSerializer.Serialize(data, _jsonOptions));
    }
}