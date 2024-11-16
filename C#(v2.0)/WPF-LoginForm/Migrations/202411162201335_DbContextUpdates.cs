namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbContextUpdates : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Repairs", "CustomerID", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Repairs", "CustomerID", c => c.Int(nullable: false));
        }
    }
}
