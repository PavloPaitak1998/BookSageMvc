namespace Server_WEB_Programming.Lab2.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        IdBook = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.IdBook);
            
            CreateTable(
                "dbo.Sages",
                c => new
                    {
                        IdSage = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Age = c.Int(nullable: false),
                        Photo = c.Binary(),
                        City = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.IdSage);
            
            CreateTable(
                "dbo.SageBook",
                c => new
                    {
                        IdSage = c.Int(nullable: false),
                        IdBook = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdSage, t.IdBook })
                .ForeignKey("dbo.Sages", t => t.IdSage, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.IdBook, cascadeDelete: true)
                .Index(t => t.IdSage)
                .Index(t => t.IdBook);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SageBook", "IdBook", "dbo.Books");
            DropForeignKey("dbo.SageBook", "IdSage", "dbo.Sages");
            DropIndex("dbo.SageBook", new[] { "IdBook" });
            DropIndex("dbo.SageBook", new[] { "IdSage" });
            DropTable("dbo.SageBook");
            DropTable("dbo.Sages");
            DropTable("dbo.Books");
        }
    }
}
