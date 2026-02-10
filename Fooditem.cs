csharp PROGRAMMINGPROJECT 2\FoodItem.cs
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class FoodItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public FoodItem() { }

        public FoodItem(string name, string description, double price)
        {
            Name = name ?? string.Empty;
            Description = description ?? string.Empty;
            Price = price;
        }

        public override string ToString() => $"{Name}: {Description} - ${Price:F2}";
    }
}