csharp PROGRAMMINGPROJECT 2\OrderFoodItem.cs
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class OrderFoodItem
    {
        public FoodItem Item { get; set; }
        public int Quantity { get; set; }

        public OrderFoodItem() { }

        public OrderFoodItem(FoodItem item, int qty)
        {
            Item = item;
            Quantity = Math.Max(1, qty);
        }

        public double CalculateSubtotal() => (Item?.Price ?? 0.0) * Quantity;
    }
}