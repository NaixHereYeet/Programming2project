using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();
        public Menu(string id, string name) { MenuId = id; MenuName = name; }
        public void AddFoodItem(FoodItem item) => FoodItems.Add(item);
    }
}