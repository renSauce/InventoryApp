```mermaid
classDiagram
  direction TB

  class Item {
    <<abstract>>
    +int Id
    +string Name
    +decimal PricePerUnit
    +uint InventoryLocation
    +decimal Quantity
  }
  class UnitItem { +decimal Weight }
  class BulkItem { +string MeasurementUnit }
  Item <|-- UnitItem
  Item <|-- BulkItem

  class OrderLine {
    +int Id
    +int ItemId
    +Item Item
    +double Quantity
    +decimal LineTotal
  }

  class Order {
    +int Id
    +DateTime Time
    +List~OrderLine~ OrderLines
    +decimal Total
    +string Summary
    +int? QueuedOrderBookId
    +int? ProcessedOrderBookId
  }
  Order "1" o-- "1..*" OrderLine
  OrderLine --> Item

  class Inventory {
    +int Id
    +List~Item~ Stock
    +CanFulfill(Order) bool
    +Deduct(Order) void
  }

  class OrderBook {
    +int Id
    +List~Order~ QueuedOrders
    +List~Order~ ProcessedOrders
    +AttachInventory(Inventory) void
    +QueueOrder(Order) void
    +ProcessNextOrder() bool
    +ProcessNextOrderAndReturnLines() List~OrderLine~?
    +TotalRevenue : decimal
  }
  OrderBook "1" o-- "0..*" Order : QueuedOrders
  OrderBook "1" o-- "0..*" Order : ProcessedOrders

  class Customer {
    +string Name
    +List~Order~ Orders
    +CreateOrder(OrderBook, Order) void
  }

  class Robot {
    +string RobotIpAddress
    +string ControlBoxIpAddress
    +SendUrscript(string) void
  }
  class ItemSorterRobot { +PickUp(uint) void +UrscriptTemplate$ string }
  Robot <|-- ItemSorterRobot

  OrderBook --> Inventory : attaches at startup
  Customer --> Order
  MainWindow --> ItemSorterRobot : sets IPs from GUI
