using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Modules.Services;

namespace OrderService.Modules.Orders
{
    class Order
    {
        public string Name { get; set; }
        public DateTime OrderTime { get; } = DateTime.Now;
        public List<Service> Services { get; set; }
    }
}
