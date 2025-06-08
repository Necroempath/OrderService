using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MessagePack;
using OrderService.Models.Orders;

namespace OrderService.Models.Users;

[MessagePackObject]
public class UserProfile
{
    [JsonIgnore]
    private List<Order> _orders = new();
    [Key(0)]
    public required string Username { get; set; }

    [XmlIgnore]
    [IgnoreMember]
    public IReadOnlyList<Order> Orders
    {
        get => _orders;
        set => _orders = value as List<Order> ?? new();
    }
    
    [JsonIgnore]
    [XmlArray("Orders")]
    [XmlArrayItem("Order")]
    [Key(1)]
    public List<Order> SerializableOrders
    {
        get => _orders;
        set => _orders = value;
    }

    public void AddOrder(Order order) => _orders.Add(order);
    public void ClearOrder()
    {
        _orders.Clear();
    }
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