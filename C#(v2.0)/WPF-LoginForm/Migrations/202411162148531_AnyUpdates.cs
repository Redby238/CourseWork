namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnyUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Contracts", "Name", c => c.String());
            AddColumn("dbo.Contracts", "Customer_CustomerID", c => c.Int());
            AddColumn("dbo.Customers", "ContactName", c => c.String());
            AddColumn("dbo.Purchases", "Supplier_SupplierID", c => c.Int());
            CreateIndex("dbo.Contracts", "Customer_CustomerID");
            CreateIndex("dbo.Purchases", "Supplier_SupplierID");
            AddForeignKey("dbo.Purchases", "Supplier_SupplierID", "dbo.Suppliers", "SupplierID");
            AddForeignKey("dbo.Contracts", "Customer_CustomerID", "dbo.Customers", "CustomerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Contracts", "Customer_CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Purchases", "Supplier_SupplierID", "dbo.Suppliers");
            DropIndex("dbo.Purchases", new[] { "Supplier_SupplierID" });
            DropIndex("dbo.Contracts", new[] { "Customer_CustomerID" });
            DropColumn("dbo.Purchases", "Supplier_SupplierID");
            DropColumn("dbo.Customers", "ContactName");
            DropColumn("dbo.Contracts", "Customer_CustomerID");
            DropColumn("dbo.Contracts", "Name");
        }
    }
}
