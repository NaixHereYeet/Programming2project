csharp PROGRAMMINGPROJECT 2\Program.cs
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;

namespace PROGRAMMINGPROJECT_2
{
    class Program
    {
        static List<Restaurant> restaurants = new List<Restaurant>();
        static List<Customer> customers = new List<Customer>();
        static List<Order> allOrders = new List<Order>();
        static List<SpecialOffer> specialOffers = new List<SpecialOffer>();

        static void Main()
        {
            Console.WriteLine("Welcome to the Gruberoo Food Delivery System");

            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems.csv");
            LoadCustomers("customers.csv");
            LoadOrders("orders.csv");
            LoadSpecialOffers("specialoffers.csv");

            Console.WriteLine($"{restaurants.Count} restaurants loaded!");
            int foodCount = restaurants.Sum(r => r.RestaurantMenu?.FoodItems.Count ?? 0);
            Console.WriteLine($"{foodCount} food items loaded!!!");
            Console.WriteLine($"{customers.Count} customers loaded!!!");
            Console.WriteLine($"{allOrders.Count} orders loaded!!!!");

            while (true)
            {
                Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
                Console.WriteLine("1. List all restaurants and menu items");
                Console.WriteLine("2. List all order");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");
                var choice = Console.ReadLine()?.Trim();

                if (choice == "1") DisplayRestaurantsAndMenus();
                else if (choice == "2") DisplayAllOrders();
                else if (choice == "0") break;
                else Console.WriteLine("Invalid option. Try again.");
            }
        }

        static void LoadRestaurants(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1);
            foreach (var line in lines)
            {
                var p = SplitCsvLine(line);
                if (p.Length < 3) continue;
                restaurants.Add(new Restaurant(p[0].Trim(), p[1].Trim(), p[2].Trim()));
            }
        }

        static void LoadFoodItems(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1);
            foreach (var line in lines)
            {
                var p = SplitCsvLine(line);
                if (p.Length < 4) continue;
                var rest = restaurants.FirstOrDefault(r => r.RestaurantId == p[0].Trim());
                if (rest == null) continue;

                if (!double.TryParse(p[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double price)) continue;
                var item = new FoodItem(p[1].Trim(), p[2].Trim(), price);
                rest.RestaurantMenu.AddFoodItem(item);
            }
        }

        static void LoadCustomers(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1);
            foreach (var line in lines)
            {
                var p = SplitCsvLine(line);
                if (p.Length < 2) continue;
                customers.Add(new Customer(p[0].Trim(), p[1].Trim()));
            }
        }

        static void LoadOrders(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1);
            foreach (var line in lines)
            {
                var p = SplitCsvLine(line);
                if (p.Length < 9) continue;

                // columns: OrderId, CustomerEmail, RestaurantId, Date(dd/MM/yyyy), Time(HH:mm), ... , Total, Status
                var orderId = p[0].Trim();
                var custEmail = p[1].Trim();
                var restId = p[2].Trim();
                var datePart = p[3].Trim();
                var timePart = p[4].Trim();
                if (!DateTime.TryParseExact($"{datePart} {timePart}", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    if (!DateTime.TryParse($"{datePart} {timePart}", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        continue;
                }

                if (!double.TryParse(p[7], NumberStyles.Any, CultureInfo.InvariantCulture, out double total)) continue;
                var status = p[8].Trim();

                var cust = customers.FirstOrDefault(c => string.Equals(c.Email, custEmail, StringComparison.OrdinalIgnoreCase));
                var rest = restaurants.FirstOrDefault(r => r.RestaurantId == restId);

                var order = new Order(orderId, cust, rest, dt, total, status);
                allOrders.Add(order);
                if (cust != null) cust.AddOrder(order);
                if (rest != null) rest.OrderQueue.Enqueue(order);
            }
        }

        static void LoadSpecialOffers(string filePath)
        {
            if (!File.Exists(filePath)) return;
            var lines = File.ReadAllLines(filePath, Encoding.UTF8).Skip(1);
            foreach (var line in lines)
            {
                var p = SplitCsvLine(line);
                if (p.Length < 4) continue;
                if (!double.TryParse(p[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double discount)) discount = 0.0;
                specialOffers.Add(new SpecialOffer(p[0].Trim(), p[1].Trim(), p[2].Trim(), discount));
            }
        }

        static void DisplayRestaurantsAndMenus()
        {
            Console.WriteLine("\nAll Restaurants and Menu Items");
            Console.WriteLine("==============================");
            foreach (var res in restaurants)
            {
                Console.WriteLine($"Restaurant: {res.Name} ({res.RestaurantId})");
                foreach (var item in res.RestaurantMenu.FoodItems)
                {
                    Console.WriteLine($"   - {item.Name}: {item.Description} - ${item.Price:F2}");
                }
                Console.WriteLine();
            }
        }

        static void DisplayAllOrders()
        {
            Console.WriteLine("\nAll Orders");
            Console.WriteLine("==========");
            Console.WriteLine("{0,-10} {1,-15} {2,-18} {3,-20} {4,-10} {5}",
                "Order ID", "Customer", "Restaurant", "Delivery Date/Time", "Amount", "Status");
            Console.WriteLine(new string('-', 95));

            foreach (var o in allOrders)
            {
                var custName = o.Customer?.Name ?? "Unknown";
                var restName = o.Restaurant?.Name ?? "Unknown";
                Console.WriteLine("{0,-10} {1,-15} {2,-18} {3,-20} ${4,-10:F2} {5}",
                    o.OrderId, custName, restName, o.DeliveryDateTime.ToString("dd/MM/yyyy HH:mm"), o.TotalAmount, o.OrderStatus);
            }
        }

        // Robust CSV splitter for quoted fields/doubled quotes
        static string[] SplitCsvLine(string line)
        {
            if (line == null) return Array.Empty<string>();
            var fields = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < line.Length && line[i + 1] == '"')
                        {
                            sb.Append('"');
                            i++;
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                else
                {
                    if (c == '"') inQuotes = true;
                    else if (c == ',')
                    {
                        fields.Add(sb.ToString());
                        sb.Clear();
                    }
                    else sb.Append(c);
                }
            }

            fields.Add(sb.ToString());
            return fields.ToArray();
        }
    }
}