classDiagram
    direction TB

    class Item {
        <<abstract>>
        +string Name
        +decimal PricePerUnit
        +uint InventoryLocation
    }
    class UnitItem { +double Weight }
    class BulkItem { +MeasurementUnit MeasurementUnit }
    Item <|-- UnitItem
    Item <|-- BulkItem

    class OrderLine { +Item Item +double Quantity +decimal LineTotal }
    class Order { +Guid Id +DateTime Time +IReadOnlyList~OrderLine~ OrderLines +decimal Total +string Summary }
    Order "1" o-- "1..*" OrderLine

    class Inventory {
        -Dictionary~Item,double~ stock
        +Set(Item,double) void
        +CanFulfill(Order) bool
        +Deduct(Order) void
    }

    class OrderBook {
        +ObservableCollection~Order~ QueuedOrders
        +ObservableCollection~Order~ ProcessedOrders
        +QueueOrder(Order) void
        +ProcessNextOrder() bool
        +ProcessNextOrderAndReturnLines() IReadOnlyList~OrderLine~?
        +TotalRevenue() decimal
    }

    class Customer { +string Name +List~Order~ Orders +CreateOrder(OrderBook,Order) void }

    class Robot {
        +string RobotIpAddress
        +string ControlBoxIpAddress
        +SendUrscript(string) void
    }

    class ItemSorterRobot {
        +PickUp(uint) void
        +UrscriptTemplate$ string
    }
    Robot <|-- ItemSorterRobot

    OrderBook --> Inventory
    Customer --> Order
    MainWindow --> ItemSorterRobot : sets IPs from GUI
