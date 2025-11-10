using InventoryApp.Models;

namespace InventoryApp.Data
{
    public static class DatabaseSeeder
    {
        public static void EnsureCreatedAndSeed(InventoryDbContext db)
        {
            db.Database.EnsureCreated();
            if (db.Inventories.Any()) return;

            // Items / Inventory 
            var screw = new UnitItem { Name = "M3 screw", PricePerUnit = 1m, InventoryLocation = 1, Quantity = 1000, Weight = 0.001m };
            var nut = new UnitItem { Name = "M3 nut", PricePerUnit = 1.5m, InventoryLocation = 2, Quantity = 300 };
            var pen = new UnitItem { Name = "Pen", PricePerUnit = 1m, InventoryLocation = 3, Quantity = 500 };

            var inventory = new Inventory
            {
                Stock = new List<Item> { screw, nut, pen }
            };
            db.Inventories.Add(inventory);

            var now = DateTime.Now;

            var o1 = new Order
            {
                Time = now.AddMinutes(-30),
                Customer = "Mogens",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = screw, Quantity = 3 },
                    new() { Item = nut,   Quantity = 2 }
                }
            };

            var o2 = new Order
            {
                Time = now.AddMinutes(-20),
                Customer = "Niels",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = pen,   Quantity = 10 },
                    new() { Item = screw, Quantity = 1 }
                }
            };

            var o3 = new Order
            {
                Time = now.AddMinutes(-10),
                Customer = "Hans",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = nut,   Quantity = 4 }
                }
            };

            var o4 = new Order
            {
                Time = now.AddMinutes(-5),
                Customer = "Victor",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = pen,   Quantity = 5 },
                    new() { Item = nut,   Quantity = 1 }
                }
            };

            var o5 = new Order
            {
                Time = now.AddMinutes(-2),
                Customer = "Peter",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = screw, Quantity = 2 },
                    new() { Item = pen,   Quantity = 3 }
                }
            };

            var o6 = new Order
            {
                Time = now,
                Customer = "Simon",
                OrderLines = new List<OrderLine>
                {
                    new() { Item = nut,   Quantity = 2 }
                }
            };

            // Single OrderBook; all orders start in Queued 
            var orderBook = new OrderBook();
            orderBook.AttachInventory(inventory);

            orderBook.QueuedOrders.Add(o1);
            orderBook.QueuedOrders.Add(o2);
            orderBook.QueuedOrders.Add(o3);
            orderBook.QueuedOrders.Add(o4);
            orderBook.QueuedOrders.Add(o5);
            orderBook.QueuedOrders.Add(o6);

            db.OrderBooks.Add(orderBook);
            db.SaveChanges();
        }
    }
}
