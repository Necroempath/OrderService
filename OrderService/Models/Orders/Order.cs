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

        public string Print(int index)
        {
            StringBuilder builder = new();
            
            builder.AppendLine($"{index + 1}. Order: {Name}");

            for (int i = 0; i < _services.Count; i++)
            {
                builder.AppendLine($"\t{index + 1}.{i + 1} Service: {_services[i].Description}\n\tPrice: {_services[i].Price}");
            }
            
            builder.AppendLine($"Order time: {OrderTime}");
            return builder.ToString();
        }
    }
}
