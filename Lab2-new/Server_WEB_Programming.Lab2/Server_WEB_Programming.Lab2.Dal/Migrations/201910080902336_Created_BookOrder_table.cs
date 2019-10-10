namespace Server_WEB_Programming.Lab2.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Created_BookOrder_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookOrders",
                c => new
                    {
                        BookOrderId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BookOrderId)
                .ForeignKey("dbo.Books", t => t.BookId, cascadeDelete: true)
                .Index(t => t.BookId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookOrders", "BookId", "dbo.Books");
            DropIndex("dbo.BookOrders", new[] { "BookId" });
            DropTable("dbo.BookOrders");
        }
    }
}
