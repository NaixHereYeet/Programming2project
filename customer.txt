csharp PROGRAMMINGPROJECT 2\Customer.cs
using System;
using System.Collections.Generic;

namespace PROGRAMMINGPROJECT_2
{
    internal class Customer
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<Order> OrderList { get; set; } = new List<Order>();

        public Customer() { }

        public Customer(string name, string email)
        {
            Name = name ?? string.Empty;
            Email = email ?? string.Empty;
        }

        public void AddOrder(Order o)
        {
            if (o != null) OrderList.Add(o);
        }
    }
}