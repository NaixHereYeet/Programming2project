using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class Order
    {
        public string OrderId { get; set; }
        public Customer Customer { get; set; }
        public Restaurant Restaurant { get; set; }
        public DateTime DeliveryDateTime { get; set; }
        public string DeliveryAddress { get; set; }
        public string SpecialRequest { get; set; }
        public string PaymentMethod { get; set; }
        public double TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public List<OrderFoodItem> OrderedItems { get; set; } = new List<OrderFoodItem>();

        public Order(string id, Customer c, Restaurant r, DateTime dt, double total, string status)
        { OrderId = id; Customer = c; Restaurant = r; DeliveryDateTime = dt; TotalAmount = total; OrderStatus = status; }
    }
}