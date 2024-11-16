namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupplierUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "ContactName", c => c.String());
            AddColumn("dbo.Suppliers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Suppliers", "Name");
            DropColumn("dbo.Suppliers", "ContactName");
        }
    }
}
