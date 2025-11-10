namespace InventoryApp.Models
{
    public class OrderBook
    {
        public int Id { get; set; }
        public List<Order> QueuedOrders { get; set; } = new();
        public List<Order> ProcessedOrders { get; set; } = new();

        private Inventory _inventory = new();

        public void AttachInventory(Inventory inv) => _inventory = inv;

        public void QueueOrder(Order order) => QueuedOrders.Add(order);

        public bool ProcessNextOrder()
        {
            if (QueuedOrders.Count == 0) return false;
            var order = QueuedOrders[0];

            // check stock in the attached, tracked inventory
            foreach (var line in order.OrderLines)
            {
                var item = _inventory.Stock.First(i => i.Id == line.ItemId);
                if (item.Quantity < (decimal)line.Quantity) return false;
            }

            // deduct
            foreach (var line in order.OrderLines)
            {
                var item = _inventory.Stock.First(i => i.Id == line.ItemId);
                item.Quantity -= (decimal)line.Quantity;
            }

            QueuedOrders.RemoveAt(0);
            order.QueuedOrderBook = null;
            order.QueuedOrderBookId = null;

            ProcessedOrders.Add(order);
            order.ProcessedOrderBook = this;
            order.ProcessedOrderBookId = this.Id;

            return true;
        }

        public List<OrderLine>? ProcessNextOrderAndReturnLines()
        {
            if (!ProcessNextOrder()) return null;
            return ProcessedOrders.Last().OrderLines;
        }

        public decimal TotalRevenue => ProcessedOrders.Sum(o => o.Total);
        public decimal TotalRevenueLegacy() => TotalRevenue; 
    }
}
