namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Repairs", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Repairs", "Description");
        }
    }
}
