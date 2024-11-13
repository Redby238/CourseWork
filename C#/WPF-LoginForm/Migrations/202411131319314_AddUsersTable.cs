 namespace WPF_LoginForm.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 256),
                        Email = c.String(maxLength: 256),
                        PasswordHash = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true)
                .Index(t => t.Email, unique: true);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "Username" });
            DropTable("dbo.Users");
        }
    }
}
