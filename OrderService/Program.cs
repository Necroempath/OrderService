using System.Text.Json;
using OrderService.Models.User;

namespace OrderService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            App app = new App();
            app.Run();
        }
    }
}
