namespace KidsClothes.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeProductIdInProductColors : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductColors", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductColors", new[] { "ProductId" });
            RenameColumn(table: "dbo.ProductColors", name: "ProductId", newName: "Product_Id");
            AlterColumn("dbo.ProductColors", "Product_Id", c => c.Int());
            CreateIndex("dbo.ProductColors", "Product_Id");
            AddForeignKey("dbo.ProductColors", "Product_Id", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductColors", "Product_Id", "dbo.Products");
            DropIndex("dbo.ProductColors", new[] { "Product_Id" });
            AlterColumn("dbo.ProductColors", "Product_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ProductColors", name: "Product_Id", newName: "ProductId");
            CreateIndex("dbo.ProductColors", "ProductId");
            AddForeignKey("dbo.ProductColors", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
        }
    }
}
