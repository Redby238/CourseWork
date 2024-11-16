namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelChange : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Repairs", "CustomerID");
            AddForeignKey("dbo.Repairs", "CustomerID", "dbo.Customers", "CustomerID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Repairs", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Repairs", new[] { "CustomerID" });
        }
    }
}
