using System.Xml.Serialization;
using OrderService.Models.Users;

namespace OrderService.Repositories;

class XmlDataRepository<T>(string fileName) : IDataRepository<T> where T : class
{
    public T? LoadData()
    {
        using FileStream file = new (fileName, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new (file);
        
        var serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(reader) as T;
    }

    public void SaveData(T data)
    {
        using FileStream file = new (fileName, FileMode.OpenOrCreate, FileAccess.Write);
        using StreamWriter writer = new (file);

        var serializer = new XmlSerializer(data.GetType());
        serializer.Serialize(writer, data);
    }
}