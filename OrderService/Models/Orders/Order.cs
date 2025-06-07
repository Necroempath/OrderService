using System.Text;
using OrderService.Models.Services;

namespace OrderService.Models.Orders
{
    public class Order
    {
        private List<Service> _services = new();
        public required string Name { get; init; }
        public DateTime OrderTime { get; } = DateTime.Now;
        
        public IReadOnlyList<Service> Services 
        { 
            get => _services;
            set => _services = value as List<Service> ?? new ();
        }

        public override string ToString()
        {
            StringBuilder builder = new();
            
            builder.AppendLine($"Order name: {Name}");
            
            foreach (Service service in _services)
            {
                builder.AppendLine(service.ToString());
            }
            builder.AppendLine($"Order time: {OrderTime}");
            
            return builder.ToString();
        }
    }
}
