namespace OrderService.Models.Users
{
    public class UserCredentials
    {
        public required string Username { get; init; }
        public required string HashPassword { get; init; }
        public required string Salt { get; init; }
    }
}
