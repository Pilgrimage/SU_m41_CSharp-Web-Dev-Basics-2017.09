namespace Lab05_Shop
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using (ShopDbContext db = new ShopDbContext())
            {
                PrepareDatabase(db);
                FillSalesmen(db);
                FillItems(db);
                ProcessCommands(db);

                //PrintSalesmenWithCustomerCount(db);               // for problem 5
                //PrintCustomersWithOrdersAndReviewsCount(db);      // for problem 6
                //PrintCustomerOrdersAndReviews(db);                // for problem 7
                //PrintCustomerData(db);                            // for problem 8
                PrintOrdersWithMoreThanOneItem(db);                 // for problem 9
            }
        }


        private static void PrepareDatabase(ShopDbContext db)
        {
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        }

        private static void FillSalesmen(ShopDbContext db)
        {
            string[] sMans = Console.ReadLine().Split(';');

            foreach (string name in sMans)
            {
                db.Salesmen.Add(new Salesman { Name = name });
            }

            db.SaveChanges();
        }

        private static void FillItems(ShopDbContext db)
        {
            while (true)
            {
                string inputLine = Console.ReadLine();

                if (inputLine == "END")
                {
                    break;
                }

                string[] parts = inputLine.Split(';');
                string itemName = parts[0];
                decimal itemPrice = decimal.Parse(parts[1]);

                db.Items.Add(new Item
                {
                    Name = itemName,
                    Price = itemPrice
                });
            }

            db.SaveChanges();
        }

        private static void ProcessCommands(ShopDbContext db)
        {
            while (true)
            {
                string inputLine = Console.ReadLine();

                if (inputLine == "END")
                {
                    break;
                }

                string[] parts = inputLine.Split('-');
                string command = parts[0];
                string arguments = parts[1];

                switch (command)
                {
                    case "register":
                        RegisterCustomer(db, arguments);
                        break;

                    case "order":
                        SaveOrder(db, arguments);

                        break;

                    case "review":
                        SaveReview(db, arguments);

                        break;

                    default:
                        break;
                }
            }
        }

        private static void RegisterCustomer(ShopDbContext db, string arguments)
        {
            string[] parts = arguments.Split(';');
            string customerName = parts[0];
            int salesmanId = int.Parse(parts[1]);

            db.Customers.Add(new Customer
            {
                Name = customerName,
                SalesmanId = salesmanId
            });

            db.SaveChanges();
        }

        private static void SaveOrder(ShopDbContext db, string arguments)
        {
            string[] parts = arguments.Split(';');
            int customerId = int.Parse(parts[0]);

            Order order = new Order { CustomerId = customerId };

            for (int i = 1; i < parts.Length; i++)
            {
                int itemId = int.Parse(parts[i]);

                order.ItemsInOrder.Add(new OrderItem
                {
                    ItemId = itemId
                });
            }

            db.Orders.Add(order);

            db.SaveChanges();
        }

        private static void SaveReview(ShopDbContext db, string arguments)
        {
            string[] parts = arguments.Split(';');

            int customerId = int.Parse(parts[0]);
            int itemId = int.Parse(parts[1]);

            db.Reviews.Add(new Review
            {
                CustomerId = customerId,
                ItemId = itemId
            });

            db.SaveChanges();
        }


        private static void PrintSalesmenWithCustomerCount(ShopDbContext db)
        {
            //var salesmenData = db.Salesmen.Include(s => s.Customers).ToList();    // Brutal way

            var salesmenData = db
                .Salesmen
                .Select(s => new
                {
                    s.Name,
                    CustomersCount = s.Customers.Count
                })
                .OrderByDescending(s => s.CustomersCount)
                .ThenBy(s => s.Name)
                .ToList();

            foreach (var salesman in salesmenData)
            {
                Console.WriteLine($"{salesman.Name} - {salesman.CustomersCount} customers");
            }

        }

        private static void PrintCustomersWithOrdersAndReviewsCount(ShopDbContext db)
        {
            // Version with Include
            //var customersData = db
            //    .Customers
            //    .Include(c=>c.Orders)
            //    .Include(c=>c.Reviews)
            //    .OrderByDescending(c => c.Orders.Count)
            //    .ThenByDescending(c => c.Reviews.Count)
            //    .ToList();

            // Version with projection
            var customersData = db
                .Customers
                .Select(c => new
                {
                    c.Name,
                    OrdersCount = c.Orders.Count,
                    ReviewsCount = c.Reviews.Count
                })
                .OrderByDescending(c => c.OrdersCount)
                .ThenByDescending(c => c.ReviewsCount)
                .ToList();

            foreach (var customer in customersData)
            {
                Console.WriteLine(customer.Name);
                Console.WriteLine($"Orders: {customer.OrdersCount}");
                Console.WriteLine($"Reviews: {customer.ReviewsCount}");
            }
        }

        private static void PrintCustomerOrdersAndReviews(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            //var customerData = db
            //    .Customers
            //    .Include(c => c.Orders)
            //        .ThenInclude(o => o.Select(or => or.ItemsInOrder))
            //    .Include(c=>c.Reviews)
            //    .Where(c => c.Id == customerId);
            
            var customerData = db
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    Orders = c
                        .Orders
                        .Select(o => new
                        {
                            o.Id,
                            ItemCount = o.ItemsInOrder.Count
                        })
                        .OrderBy(o => o.Id),
                    Reviews = c.Reviews.Count
                })
                .FirstOrDefault();

            foreach (var order in customerData.Orders)
            {
                Console.WriteLine($"order {order.Id}: {order.ItemCount} items");
            }

            Console.WriteLine($"reviews: {customerData.Reviews}");
        }

        private static void PrintCustomerData(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            var customerData = db
                .Customers
                .Where(c => c.Id == customerId)
                .Select(c => new
                {
                    c.Name,
                    OrdersCount = c.Orders.Count,
                    ReviewsCount = c.Reviews.Count,
                    SalecmanName = c.Salesman.Name
                })
                .FirstOrDefault();

            Console.WriteLine($"Customer: {customerData.Name}");
            Console.WriteLine($"Orders count:{customerData.OrdersCount}");
            Console.WriteLine($"Reviews count: {customerData.ReviewsCount}");
            Console.WriteLine($"Salesman: {customerData.SalecmanName}");
        }

        private static void PrintOrdersWithMoreThanOneItem(ShopDbContext db)
        {
            int customerId = int.Parse(Console.ReadLine());

            //int orders = db
            //    .Orders
            //    .Where(o => o.CustomerId == customerId)
            //    .Where(o => o.ItemsInOrder.Count > 1)
            //    .Count();

            int orders = db
                .Orders
                .Count(o => o.CustomerId == customerId && 
                            o.ItemsInOrder.Count > 1);


            Console.WriteLine($"Orders count: {orders}");
        }
    }
}
