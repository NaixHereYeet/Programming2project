using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class Restaurant
    {
        public string RestaurantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Menu RestaurantMenu { get; set; } = new Menu("M001", "Main");
        public Queue<Order> OrderQueue { get; set; } = new Queue<Order>();
        public Restaurant(string id, string name, string email) { RestaurantId = id; Name = name; Email = email; }
    }
}