namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchemaUpdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropTable("dbo.Users");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        PasswordHash = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Users", "Username", unique: true);
        }
    }
}
