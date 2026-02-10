using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class FoodItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public FoodItem(string name, string desc, double price) { Name = name; Description = desc; Price = price; }
    }
}