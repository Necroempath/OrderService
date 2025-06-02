using OrderService.Modules.Users;
using System.Text.Json;

namespace OrderService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //using FileStream fileStream = new("UsersCredentials.json", FileMode.Create);
            //using StreamWriter streamWriter = new(fileStream);
            //string userCredentialsJson = JsonSerializer.Deserialize()
            //string json = File.ReadAllText("UsersCredentials.json");
            //if(json.Length == 0)
            //{
            //    Console.WriteLine("no data");
            //}
            //else { Console.WriteLine(json); }
            List<User> users = new()
            {
                new("polly", "@123"),
                new("aurora", "#nutshell#"),
                new("james", "qwerty")
            };
            //string jsonSerialized = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            using FileStream fileStream = new("UsersCredentials.json", FileMode.Open);
            //using StreamWriter streamWriter = new(fileStream);
            //streamWriter.Write(jsonSerialized);
            using StreamReader streamReader = new(fileStream);
            string usersFile = streamReader.ReadToEnd();

            List<User> json = JsonSerializer.Deserialize<List<User>>(usersFile);
            foreach(var user in json)
            {
                Console.WriteLine(user.Username);
                Console.WriteLine();
            }
        }

        static User SignIn()
        {
            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            return new(username, password);
        }
    }
}
