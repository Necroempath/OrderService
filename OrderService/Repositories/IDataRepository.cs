using OrderService.Models.User;

namespace OrderService.Repositories;

interface IDataRepository<T> where T : class
{
    T? LoadData();
    void SaveData(T data);
}