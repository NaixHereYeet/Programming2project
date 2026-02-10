using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Order> OrderList { get; set; } = new List<Order>();
        public Customer(string name, string email) { Name = name; Email = email; }
        public void AddOrder(Order o) { if (o != null) OrderList.Add(o); }
    }
}