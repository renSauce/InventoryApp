using Avalonia.Controls;
using Avalonia.Interactivity;
using InventoryApp.Data;
using InventoryApp.Models;
using InventoryApp.Robotics;
using Microsoft.EntityFrameworkCore;

namespace InventoryApp.Views
{
    public partial class MainWindow : Window
    {
        private readonly InventoryDbContext _db = new();  
        public OrderBook OrderBook { get; private set; } = null!;

        public MainWindow()
        {
            InitializeComponent();

            // Seed once
            DatabaseSeeder.EnsureCreatedAndSeed(_db);

            // Load inventory with stock
            var inventory = _db.Inventories
                .Include(i => i.Stock)
                .First();

            // Load orderbook 
            OrderBook = _db.OrderBooks
                .Include(ob => ob.QueuedOrders)
                    .ThenInclude(o => o.OrderLines)
                        .ThenInclude(ol => ol.Item)
                .Include(ob => ob.ProcessedOrders)
                    .ThenInclude(o => o.OrderLines)
                        .ThenInclude(ol => ol.Item)
                .First();

            // Attach the real inventory so Deduct() mutates tracked Items
            OrderBook.AttachInventory(inventory);

            DataContext = this;
            RevenueText.Text = $"{OrderBook.TotalRevenue:C}";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _db.Dispose();
        }

        public void ProcessNextOrder_OnClick(object? sender, RoutedEventArgs e)
        {
            if (!OrderBook.ProcessNextOrder()) return;

            _db.SaveChanges(); // same tracked entities -> DB persists
            RevenueText.Text = $"{OrderBook.TotalRevenue:C}";
        }

        public async void ProcessWithRobot_OnClick(object? sender, RoutedEventArgs e)
        {
            var lines = OrderBook.ProcessNextOrderAndReturnLines();
            if (lines is null) return;

            var robot = new ItemSorterRobot
            {
                RobotIpAddress = RobotIpText!.Text!,
                ControlBoxIpAddress = ControlBoxIpText!.Text!
            };

            foreach (var line in lines)
            {
                for (int i = 0; i < (int)Math.Ceiling(line.Quantity); i++)
                {
                    try { robot.PickUp(line.Item.InventoryLocation); } catch { }
                    await Task.Delay(9500);
                }
            }

            _db.SaveChanges();
            RevenueText.Text = $"{OrderBook.TotalRevenue:C}";
        }
    }
}
