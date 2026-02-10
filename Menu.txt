csharp PROGRAMMINGPROJECT 2\Menu.cs
using System;
using System.Collections.Generic;

namespace PROGRAMMINGPROJECT_2
{
    internal class Menu
    {
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

        public Menu() { }

        public Menu(string id, string name)
        {
            MenuId = id ?? string.Empty;
            MenuName = name ?? string.Empty;
        }

        public void AddFoodItem(FoodItem item)
        {
            if (item != null) FoodItems.Add(item);
        }
    }
}