csharp PROGRAMMINGPROJECT 2\Order.cs
using System;
using System.Collections.Generic;
using System.Linq;

namespace PROGRAMMINGPROJECT_2
{
    internal class Order
    {
        public string OrderId { get; set; }
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderFoodItem> OrderedItems { get; set; } = new List<OrderFoodItem>();

        public Order() { }

        public Order(string id, Customer customer, Restaurant restaurant, DateTime dt, double total, string status)
        {
            OrderId = id;
            Customer = customer;
            Restaurant = restaurant;
            DeliveryDateTime = dt;
            TotalAmount = total;
            OrderStatus = status ?? string.Empty;
        }
    }
}