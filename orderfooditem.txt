using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    internal class OrderFoodItem
    {
        public FoodItem Item { get; set; }
        public int Quantity { get; set; }
        public OrderFoodItem(FoodItem item, int qty) { Item = item; Quantity = qty; }
        public double CalculateSubtotal() => (Item?.Price ?? 0.0) * Quantity;
    }
}