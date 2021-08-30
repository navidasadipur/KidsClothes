namespace KidsClothes.Infratructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addProductSizeModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductSizes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InsertUser = c.String(),
                        InsertDate = c.DateTime(),
                        UpdateUser = c.String(),
                        UpdateDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Link = c.String(),
                        Title = c.String(),
                        SizeValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductSizes", "ProductId", "dbo.Products");
            DropIndex("dbo.ProductSizes", new[] { "ProductId" });
            DropTable("dbo.ProductSizes");
        }
    }
}
