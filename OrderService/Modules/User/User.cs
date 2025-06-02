using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderService.Modules.Orders;

namespace OrderService.Modules.Users
{
    class User(string username, string password, List<Order> orderList = null)
    {

        public string Username { get; } = username;
        public string Password { get; } = password;

        public List<Order> Orders { get; set; } = orderList ?? new List<Order>();
    }
}
