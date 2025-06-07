using OrderService.Models.Users;

namespace OrderService.Repositories;

public interface IDataRepository<T> where T : class
{
    T? LoadData();
    void SaveData(T data);
}