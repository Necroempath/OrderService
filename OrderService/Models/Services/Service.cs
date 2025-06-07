namespace OrderService.Models.Services
{
    public class Service
    {
        public required string Description { get; init; }
        public required float Price { get; init; }

        public override string ToString()
        {
            return $"""
                    Service description: {Description}
                    Service price: {Price}
                    """;
                              
        }
    }
}
