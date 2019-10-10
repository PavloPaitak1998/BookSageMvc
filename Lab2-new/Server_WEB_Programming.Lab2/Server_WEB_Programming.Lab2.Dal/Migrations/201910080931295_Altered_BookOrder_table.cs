namespace Server_WEB_Programming.Lab2.Dal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Altered_BookOrder_table : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookOrders", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookOrders", "Quantity");
        }
    }
}
