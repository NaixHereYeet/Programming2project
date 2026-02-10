//Yi Xian - 1,4,6,8
//Ibraheem - 2,3,5,7

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Text;
using System;

namespace PROGRAMMINGPROJECT_2
{
    class Program
    {
        static List<Restaurant> restaurants = new List<Restaurant>();
        static List<Customer> customers = new List<Customer>();
        static List<Order> allOrders = new List<Order>();
        static Stack<Order> refundStack = new Stack<Order>();

        static void Main()
        {
           
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems.csv");
            LoadCustomers("customers.csv");
            LoadOrders("orders.csv");

            while (true)
            {
                Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
                Console.WriteLine("1. List Restaurants and Menus");
                Console.WriteLine("2. List All Orders");
                Console.WriteLine("3. Create New Order");
                Console.WriteLine("4. Process Order (Manual)");
                Console.WriteLine("5. Modify Order");
                Console.WriteLine("6. Cancel Order");
                Console.WriteLine("7. Bulk Process (Advanced A)");
                Console.WriteLine("8. Financial Report (Advanced B)");
                Console.WriteLine("0. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                if (choice == "1") DisplayRestaurantsAndMenus();
                else if (choice == "2") DisplayAllOrders();
                else if (choice == "3") CreateNewOrder();
                else if (choice == "4") ProcessOrder();
                else if (choice == "5") ModifyOrder();
                else if (choice == "6") DeleteOrder();
                else if (choice == "7") BulkProcessOrders();
                else if (choice == "8") DisplayTotalRevenue();
                else if (choice == "0") break;
            }
        }

        
        static void DisplayRestaurantsAndMenus()
        {
            foreach (var r in restaurants)
            {
                Console.WriteLine($"\n[{r.RestaurantId}] {r.Name}");
                foreach (var i in r.RestaurantMenu.FoodItems)
                    Console.WriteLine($"  - {i.Name}: ${i.Price:F2}");
            }
        }

        static void DisplayAllOrders()
        {
            Console.WriteLine("\n{0,-8} {1,-15} {2,-15} {3,-10} {4}", "ID", "Customer", "Restaurant", "Total", "Status");
            foreach (var o in allOrders)
                Console.WriteLine("{0,-8} {1,-15} {2,-15} ${3,-9:F2} {4}", o.OrderId, o.Customer?.Name, o.Restaurant?.Name, o.TotalAmount, o.OrderStatus);
        }

        static void CreateNewOrder()
        {
            Console.Write("Customer Email: "); string email = Console.ReadLine();
            var cust = customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            Console.Write("Restaurant ID: "); string rid = Console.ReadLine();
            var res = restaurants.FirstOrDefault(r => r.RestaurantId.Equals(rid, StringComparison.OrdinalIgnoreCase));

            if (cust == null || res == null) { Console.WriteLine("Invalid User/Restaurant!"); return; }

            string id = (allOrders.Count + 1001).ToString();
            Order o = new Order(id, cust, res, DateTime.Now.AddHours(2), 0, "Pending");

           
            var item = res.RestaurantMenu.FoodItems[0];
            o.OrderedItems.Add(new OrderFoodItem(item, 1));
            o.TotalAmount = item.Price + 5.00;

            allOrders.Add(o); cust.AddOrder(o); res.OrderQueue.Enqueue(o);
            Console.WriteLine($"Order {id} created successfully!");
        }

        static void ProcessOrder()
        {
            Console.Write("Enter Restaurant ID: ");
            string rid = Console.ReadLine();
            var res = restaurants.FirstOrDefault(r => r.RestaurantId == rid);
            if (res == null || res.OrderQueue.Count == 0) return;

            Order o = res.OrderQueue.Peek();
            Console.WriteLine($"Order {o.OrderId} Status: {o.OrderStatus}. [C]onfirm / [D]eliver / [R]eject?");
            string act = Console.ReadLine().ToUpper();
            if (act == "C") o.OrderStatus = "Preparing";
            else if (act == "D" && o.OrderStatus == "Preparing") { o.OrderStatus = "Delivered"; res.OrderQueue.Dequeue(); }
            else if (act == "R") { o.OrderStatus = "Rejected"; refundStack.Push(res.OrderQueue.Dequeue()); }
        }

        static void ModifyOrder()
        {
            Console.Write("Customer Email: "); string email = Console.ReadLine();
            var cust = customers.FirstOrDefault(c => c.Email == email);
            var pending = cust?.OrderList.Where(x => x.OrderStatus == "Pending").ToList();
            if (pending == null || !pending.Any()) return;
            Console.WriteLine("Pending IDs: " + string.Join(", ", pending.Select(p => p.OrderId)));
            Console.Write("Select ID: "); string id = Console.ReadLine();
            var o = pending.FirstOrDefault(x => x.OrderId == id);
            if (o != null) { Console.Write("New Address: "); o.DeliveryAddress = Console.ReadLine(); Console.WriteLine("Updated!"); }
        }

        static void DeleteOrder()
        {
            Console.Write("Customer Email: "); string email = Console.ReadLine();
            var cust = customers.FirstOrDefault(c => c.Email == email);
            Console.Write("Order ID: "); string id = Console.ReadLine();
            var o = cust?.OrderList.FirstOrDefault(x => x.OrderId == id && x.OrderStatus == "Pending");
            if (o != null) { o.OrderStatus = "Cancelled"; refundStack.Push(o); Console.WriteLine("Cancelled."); }
        }

        static void BulkProcessOrders()
        {
            var pending = allOrders.Where(o => o.OrderStatus == "Pending").ToList();
            foreach (var o in pending)
            {
                if ((o.DeliveryDateTime - DateTime.Now).TotalHours < 1) { o.OrderStatus = "Rejected"; refundStack.Push(o); }
                else o.OrderStatus = "Preparing";
            }
            Console.WriteLine("Bulk Processing Complete.");
        }

        static void DisplayTotalRevenue()
        {
            double rev = allOrders.Where(o => o.OrderStatus == "Delivered").Sum(o => o.TotalAmount - 5.00);
            double refu = refundStack.Sum(o => o.TotalAmount);
            Console.WriteLine($"Total Revenue: ${rev:F2} | Total Refunds: ${refu:F2}");
        }

       
        static void LoadRestaurants(string f) { if (File.Exists(f)) foreach (var l in File.ReadAllLines(f).Skip(1)) { var p = l.Split(','); restaurants.Add(new Restaurant(p[0], p[1], p[2])); } }
        static void LoadFoodItems(string f) { if (File.Exists(f)) foreach (var l in File.ReadAllLines(f).Skip(1)) { var p = l.Split(','); var res = restaurants.FirstOrDefault(r => r.RestaurantId == p[0]); res?.RestaurantMenu.AddFoodItem(new FoodItem(p[1], p[2], double.Parse(p[3]))); } }
        static void LoadCustomers(string f) { if (File.Exists(f)) foreach (var l in File.ReadAllLines(f).Skip(1)) { var p = l.Split(','); customers.Add(new Customer(p[0], p[1])); } }
        static void LoadOrders(string f)
        {
            if (!File.Exists(f)) return;
            foreach (var l in File.ReadAllLines(f).Skip(1))
            {
                var p = l.Split(',');
                var c = customers.FirstOrDefault(x => x.Email == p[1]);
                var r = restaurants.FirstOrDefault(x => x.RestaurantId == p[2]);
                if (c != null && r != null)
                {
                    var o = new Order(p[0], c, r, DateTime.Now, double.Parse(p[7]), p[8]);
                    allOrders.Add(o); c.AddOrder(o); r.OrderQueue.Enqueue(o);
                }
            }
        }
    }
}