using System.Runtime.Serialization.Formatters.Binary;
using MessagePack;

namespace OrderService.Repositories;

class BinaryDataRepository<T>(string fileName) : IDataRepository<T> where T : class
{
    public T LoadData()
    {
        using FileStream stream = new(fileName, FileMode.Open, FileAccess.Read);
        return MessagePackSerializer.Deserialize<T>(stream);
    }

    public void SaveData(T data)
    {
        using FileStream stream = new (fileName, FileMode.OpenOrCreate, FileAccess.Write);
        MessagePackSerializer.Serialize(stream, data);
    }
}