using System;
using System.Collections.Generic;
using System.Linq;

namespace InventoryApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public string? Customer { get; set; }
        public List<OrderLine> OrderLines { get; set; } = new();
        public int? QueuedOrderBookId { get; set; }
        public OrderBook? QueuedOrderBook { get; set; }
        public int? ProcessedOrderBookId { get; set; }
        public OrderBook? ProcessedOrderBook { get; set; }
        public decimal Total => OrderLines.Sum(l => l.LineTotal);
        public string Summary => string.Join(", ", OrderLines.Select(l => $"{l.Item.Name} x {l.Quantity}"));
    }
}
