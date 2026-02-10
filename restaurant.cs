csharp PROGRAMMINGPROJECT 2\Restaurant.cs
using System;
using System.Collections.Generic;

namespace PROGRAMMINGPROJECT_2
{
    internal class Restaurant
    {
        public string RestaurantId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // single main menu for this simplified model
        public Menu RestaurantMenu { get; set; }
        public Queue<Order> OrderQueue { get; set; } = new Queue<Order>();

        public Restaurant() { }

        public Restaurant(string id, string name, string email)
        {
            RestaurantId = id ?? string.Empty;
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
            RestaurantMenu = new Menu("M001", "Main Menu");
        }

        public override string ToString() => $"{Name} ({RestaurantId})";
    }
}