namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CustomComputers", "Name", c => c.String());
            AddColumn("dbo.CustomComputers", "Specifications", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.CustomComputers", "Specifications");
            DropColumn("dbo.CustomComputers", "Name");
        }
    }
}
