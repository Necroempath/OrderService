using OrderService.Models.Orders;

namespace OrderService.Models.Users;

public class UserProfile
{
    private List<Order> _orders = new();
    
    public required string Username { get; init; }

    public IReadOnlyList<Order> Orders
    {
        get => _orders;
        set => _orders = value as List<Order> ?? new();
    }

    public void AddOrder(Order order)
    {
        _orders.Add(order);
    }

    public void RemoveOrder(Order order)
    {
        _orders.Remove(order);
    }
}