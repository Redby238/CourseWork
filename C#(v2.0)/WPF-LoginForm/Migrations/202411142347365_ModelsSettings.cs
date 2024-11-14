namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelsSettings : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        ContractID = c.Int(nullable: false, identity: true),
                        SupplierID = c.Int(nullable: false),
                        ContractDate = c.DateTime(nullable: false),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ContractID)
                .ForeignKey("dbo.Suppliers", t => t.SupplierID)
                .Index(t => t.SupplierID);
            
            CreateTable(
                "dbo.CustomComputerComponents",
                c => new
                    {
                        CustomComputerComponentID = c.Int(nullable: false, identity: true),
                        CustomComputerID = c.Int(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomComputerComponentID)
                .ForeignKey("dbo.CustomComputers", t => t.CustomComputerID)
                .Index(t => t.CustomComputerID);
            
            CreateTable(
                "dbo.CustomComputers",
                c => new
                    {
                        CustomComputerID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CustomComputerID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Purchases",
                c => new
                    {
                        PurchaseID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        CustomerID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.PurchaseID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.ProductID)
                .Index(t => t.CustomerID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Customers", t => t.CustomerID)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.Repairs",
                c => new
                    {
                        RepairID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        RepairDate = c.DateTime(nullable: false),
                        Status = c.String(),
                        IsUnderWarranty = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RepairID)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID);
            
            AddColumn("dbo.Products", "Supplier_SupplierID", c => c.Int());
            CreateIndex("dbo.Products", "Supplier_SupplierID");
            AddForeignKey("dbo.Products", "Supplier_SupplierID", "dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Repairs", "ProductID", "dbo.Products");
            DropForeignKey("dbo.CustomComputers", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Purchases", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Purchases", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Purchases", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.CustomComputerComponents", "CustomComputerID", "dbo.CustomComputers");
            DropForeignKey("dbo.Contracts", "SupplierID", "dbo.Suppliers");
            DropForeignKey("dbo.Products", "Supplier_SupplierID", "dbo.Suppliers");
            DropIndex("dbo.Repairs", new[] { "ProductID" });
            DropIndex("dbo.Orders", new[] { "CustomerID" });
            DropIndex("dbo.Purchases", new[] { "OrderID" });
            DropIndex("dbo.Purchases", new[] { "CustomerID" });
            DropIndex("dbo.Purchases", new[] { "ProductID" });
            DropIndex("dbo.CustomComputers", new[] { "CustomerID" });
            DropIndex("dbo.CustomComputerComponents", new[] { "CustomComputerID" });
            DropIndex("dbo.Products", new[] { "Supplier_SupplierID" });
            DropIndex("dbo.Contracts", new[] { "SupplierID" });
            DropColumn("dbo.Products", "Supplier_SupplierID");
            DropTable("dbo.Repairs");
            DropTable("dbo.Orders");
            DropTable("dbo.Purchases");
            DropTable("dbo.CustomComputers");
            DropTable("dbo.CustomComputerComponents");
            DropTable("dbo.Contracts");
            AddForeignKey("dbo.Products", "SupplierID", "dbo.Suppliers", "SupplierID", cascadeDelete: true);
        }
    }
}
