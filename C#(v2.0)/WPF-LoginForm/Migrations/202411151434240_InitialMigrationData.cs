namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigrationData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repairs", "CustomerID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repairs", "CustomerID");
        }
    }
}
