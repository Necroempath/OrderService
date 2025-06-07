using System.Text;
using System.Text.Json.Serialization;
using OrderService.Models.Orders;

namespace OrderService.Models.Users;

public class UserProfile
{
    [JsonIgnore]
    private List<Order> _orders = new();
    
    public required string Username { get; init; }

    public IReadOnlyList<Order> Orders
    {
        get => _orders;
        set => _orders = value as List<Order> ?? new();
    }

    public void AddOrder(Order order) => _orders.Add(order);

    public void RemoveOrder(Order order) => _orders.Remove(order);

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.AppendLine($"Username: {Username}");
        
        foreach (Order order in _orders)
        {
            builder.AppendLine(order.ToString());
        }
        
        return builder.ToString();
    }
}