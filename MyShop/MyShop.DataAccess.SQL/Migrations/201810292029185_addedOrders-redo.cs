namespace MyShop.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedOrdersredo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderItems", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderItems", "Image");
        }
    }
}
