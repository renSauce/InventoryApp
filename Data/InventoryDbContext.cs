using Microsoft.EntityFrameworkCore;
using InventoryApp.Models;

namespace InventoryApp.Data
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Item> Items => Set<Item>();
        public DbSet<UnitItem> UnitItems => Set<UnitItem>();
        public DbSet<BulkItem> BulkItems => Set<BulkItem>();
        public DbSet<Inventory> Inventories => Set<Inventory>();

        public DbSet<OrderBook> OrderBooks => Set<OrderBook>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderLine> OrderLines => Set<OrderLine>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=inventory.sqlite");

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Item
            b.Entity<Item>()
             .HasDiscriminator<string>("Discriminator")
             .HasValue<UnitItem>("UnitItem")
             .HasValue<BulkItem>("BulkItem");

            // Inventory -> Items
            b.Entity<Inventory>()
             .HasMany(i => i.Stock)
             .WithOne()
             .OnDelete(DeleteBehavior.Cascade);

            // Order -> OrderLines 
            b.Entity<Order>()
             .HasMany(o => o.OrderLines)
             .WithOne(ol => ol.Order)
             .HasForeignKey(ol => ol.OrderId)
             .OnDelete(DeleteBehavior.Cascade);

            // OrderLine -> Item 
            b.Entity<OrderLine>()
             .HasOne(ol => ol.Item)
             .WithMany()
             .HasForeignKey(ol => ol.ItemId)
             .OnDelete(DeleteBehavior.Restrict);

            // OrderBook -> QueuedOrders
            b.Entity<OrderBook>()
             .HasMany(ob => ob.QueuedOrders)
             .WithOne(o => o.QueuedOrderBook)
             .HasForeignKey(o => o.QueuedOrderBookId)
             .OnDelete(DeleteBehavior.NoAction);

            // OrderBook -> ProcessedOrders
            b.Entity<OrderBook>()
             .HasMany(ob => ob.ProcessedOrders)
             .WithOne(o => o.ProcessedOrderBook)
             .HasForeignKey(o => o.ProcessedOrderBookId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
