using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using InventoryApp.Models;
using System;
using System.Threading.Tasks;
using InventoryApp.Robotics;

namespace InventoryApp.Views
{
    public partial class MainWindow : Window
    {
        public OrderBook OrderBook { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            // 1) Create inventory
            var inventory = new Inventory();

            // 2) Three unit items at slots a=1, b=2, c=3
            var item1 = new UnitItem("M3 screw", 1m) { InventoryLocation = 1 };
            var item2 = new UnitItem("M3 nut", 1.5m) { InventoryLocation = 2 };
            var item3 = new UnitItem("Pen", 1m) { InventoryLocation = 3 };

            // 3) Stock levels
            inventory.Set(item1, 100);
            inventory.Set(item2, 100);
            inventory.Set(item3, 100);

            // 4) Single OrderBook instance bound to the UI
            OrderBook = new OrderBook(inventory);

            // 5) Build order lines & orders
            var orderLine1 = new OrderLine(item1, 1);
            var orderLine2 = new OrderLine(item2, 2);
            var orderLine3 = new OrderLine(item3, 1);

            var order1 = new Order(DateTime.Now - TimeSpan.FromDays(2), [orderLine1, orderLine2, orderLine3]);
            var order2 = new Order(DateTime.Now, [orderLine2]);

            // 6) Queue them via customers (as required)
            var customer1 = new Customer("Ramanda");
            var customer2 = new Customer("Totoro");
            customer1.CreateOrder(OrderBook, order1);
            customer2.CreateOrder(OrderBook, order2);

            // 7) Bind AFTER the OrderBook is populated
            DataContext = this;

            // 8) Initial revenue
            RevenueText.Text = $"{OrderBook.TotalRevenue():C}";
        }

        public void ProcessNextOrder_OnClick(object? sender, RoutedEventArgs e)
        {
            if (OrderBook.ProcessNextOrder())
                RevenueText.Text = $"{OrderBook.TotalRevenue():C}";
        }

        public async void ProcessWithRobot_OnClick(object? sender, RoutedEventArgs e)
        {
            // Process the next order AND get its lines for the robot
            var lines = OrderBook.ProcessNextOrderAndReturnLines();
            if (lines is null || lines.Count == 0)
                return;

            var robot = new ItemSorterRobot();

            foreach (var line in lines)
            {

                for (var i = 0; i < (int)Math.Ceiling(line.Quantity); i++)
                {
                    try
                    {
                        robot.PickUp(line.Item.InventoryLocation);
                    }
                    catch
                    {
                    }
                    //Task delay, so simulator works
                    await Task.Delay(9500);
                }
            }


            RevenueText.Text = $"{OrderBook.TotalRevenue():C}";
        }

    }
}
