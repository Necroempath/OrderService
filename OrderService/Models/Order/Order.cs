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
    }
}
