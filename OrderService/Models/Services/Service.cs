using MessagePack;

namespace OrderService.Models.Services
{
    [MessagePackObject]
    public class Service
    {
        [Key(0)]
        public required string Description { get; set; }
        [Key(1)]
        public required float Price { get; set; }

        public override string ToString()
        {
            return $"""
                    Service description: {Description}
                    Service price: {Price}
                    """;
                              
        }
    }

}
