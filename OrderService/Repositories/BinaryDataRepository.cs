using System.Runtime.Serialization.Formatters.Binary;

namespace OrderService.Repositories;

// class BinaryDataRepository<T>(string fileName) : IDataRepository<T>
// {
//     public List<T>? LoadData()
//     {
//         FileStream file = new(fileName, FileMode.Open, FileAccess.Read);
//         BinaryFormatter formatter = new();
//     }
// }