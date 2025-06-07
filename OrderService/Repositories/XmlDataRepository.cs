using System.Xml.Serialization;
using OrderService.Models.Users;

namespace OrderService.Repositories;

class XmlDataRepository<T> : IDataRepository<T> where T : class
{
    private readonly string _fileName;

    public XmlDataRepository(string fileName)
    {
        _fileName = fileName;
    }

    public T? LoadData()
    {
        using FileStream file = new(_fileName, FileMode.Open, FileAccess.Read);
        using StreamReader reader = new(file);

        var serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(reader) as T;
    }

    public void SaveData(T data)
    {
        using FileStream file = new(_fileName, FileMode.OpenOrCreate, FileAccess.Write);
        using StreamWriter writer = new(file);

        var serializer = new XmlSerializer(data.GetType());
        serializer.Serialize(writer, data);
    }
}