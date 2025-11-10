using System.Collections.Generic;

namespace InventoryApp.Models
{
    public class Inventory
    {
        public int Id { get; set; }
        public List<Item> Stock { get; set; } = new();
    }
}
