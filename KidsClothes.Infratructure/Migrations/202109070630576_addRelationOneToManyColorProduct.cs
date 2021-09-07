namespace KidsClothes.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRelationOneToManyColorProduct : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductColors", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductColors", new[] { "Product_Id" });
            RenameColumn(table: "dbo.ProductColors", name: "Product_Id", newName: "ProductId");
            AlterColumn("dbo.ProductColors", "ProductId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductColors", "ProductId");
            AddForeignKey("dbo.ProductColors", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductColors", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductColors", new[] { "ProductId" });
            AlterColumn("dbo.ProductColors", "ProductId", c => c.Int());
            RenameColumn(table: "dbo.ProductColors", name: "ProductId", newName: "Product_Id");
            CreateIndex("dbo.ProductColors", "Product_Id");
            AddForeignKey("dbo.ProductColors", "Product_Id", "dbo.Products", "Id");
        }
    }
}
