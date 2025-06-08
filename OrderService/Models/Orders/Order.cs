using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using MessagePack;
using OrderService.Models.Services;

namespace OrderService.Models.Orders
{
    [MessagePackObject]
    public class Order
    {
        private List<Service> _services = new();
        [Key(0)]
        public required string Name { get; set; }
        
        [XmlIgnore]
        [IgnoreMember]
        public IReadOnlyList<Service> Services 
        { 
            get => _services;
            set => _services = value as List<Service> ?? new ();
        }

        [JsonIgnore]
        [XmlArray("Services")]
        [XmlArrayItem("Service")]
        [Key(1)]
        public List<Service> SerializableServices
        {
            get => _services;
            set => _services = value;
        }
        
        [XmlIgnore]
        [IgnoreMember]
        public float TotalPrice => _services.Sum(s => s.Price);
        [Key(2)]
        public DateTime OrderTime { get; } = DateTime.Now;
        
        public string Print(int index)
        {
            StringBuilder builder = new();
            
            builder.AppendLine($"{index + 1}. Order: {Name}");

            for (int i = 0; i < _services.Count; i++)
            {
                builder.AppendLine($"\t{index + 1}.{i + 1} Service: {_services[i].Description}\n\tPrice: {_services[i].Price}");
            }
            
            builder.AppendLine($"Total price: {TotalPrice}");
            builder.AppendLine($"Order time: {OrderTime}");

            return builder.ToString();
        }
    }
}
