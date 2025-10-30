using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;

namespace InventoryApp.Models
{
    public sealed class OrderBook
    {
        public ObservableCollection<Order> QueuedOrders { get; } = [];
        public ObservableCollection<Order> ProcessedOrders { get; } = [];

        private readonly Inventory _inventory;
        private decimal _totalRevenue;

        public OrderBook(Inventory inventory) => _inventory = inventory;

        public void QueueOrder(Order order) => QueuedOrders.Add(order);

        public bool ProcessNextOrder()
        {
            if (QueuedOrders.Count == 0) return false;
            var order = QueuedOrders[0];
            if (!_inventory.CanFulfill(order)) return false;

            _inventory.Deduct(order);
            QueuedOrders.RemoveAt(0);
            ProcessedOrders.Add(order);
            _totalRevenue += order.TotalPrice();
            return true;
        }

        public decimal TotalRevenue() => _totalRevenue;

        public IReadOnlyList<OrderLine>? ProcessNextOrderAndReturnLines()
        {
            if (QueuedOrders.Count == 0) return null;
            var order = QueuedOrders[0];
            if (!_inventory.CanFulfill(order)) return null;

            _inventory.Deduct(order);
            QueuedOrders.RemoveAt(0);
            ProcessedOrders.Add(order);
            _totalRevenue += order.TotalPrice();

            return order.OrderLines;
        }
    }


}
