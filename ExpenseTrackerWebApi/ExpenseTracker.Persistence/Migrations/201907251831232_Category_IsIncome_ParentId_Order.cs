namespace ExpenseTracker.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Category_IsIncome_ParentId_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsIncomeCategory", c => c.Boolean(nullable: false));
            AddColumn("dbo.Categories", "ParentCategoryId", c => c.Int());
            AddColumn("dbo.Categories", "Order", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "ParentCategoryId");
            AddForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories", "CategoryId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentCategoryId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentCategoryId" });
            DropColumn("dbo.Categories", "Order");
            DropColumn("dbo.Categories", "ParentCategoryId");
            DropColumn("dbo.Categories", "IsIncomeCategory");
        }
    }
}
