namespace InventoryApp.Models
{
    public enum MeasurementUnit { Piece, Kilogram }

    public abstract class Item
    {
        public int Id { get; set; }               
        public string Name { get; set; } = "";    
        public decimal PricePerUnit { get; set; }
        public uint InventoryLocation { get; set; }
        public decimal Quantity { get; set; }     

        public override string ToString() => Name;
    }

    public sealed class UnitItem : Item
    {
        public decimal Weight { get; set; }
    }

    public sealed class BulkItem : Item
    {
        public string MeasurementUnit { get; set; } = "";
    }
}
