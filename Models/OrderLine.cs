namespace InventoryApp.Models
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public double Quantity { get; set; }

        public decimal LineTotal => (decimal)Quantity * Item.PricePerUnit;
    }
}
