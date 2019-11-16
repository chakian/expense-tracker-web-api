namespace ExpenseTracker.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTransactionTemplate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250, unicode: false),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Description = c.String(),
                        CategoryId = c.Int(),
                        SourceAccountId = c.Int(),
                        TargetAccountId = c.Int(),
                        BudgetId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Accounts", t => t.SourceAccountId)
                .ForeignKey("dbo.Accounts", t => t.TargetAccountId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => new { t.Name, t.BudgetId, t.UserId }, unique: true, name: "IX_TemplateName_User_Budget")
                .Index(t => t.CategoryId)
                .Index(t => t.SourceAccountId)
                .Index(t => t.TargetAccountId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TransactionTemplates", "UserId", "dbo.Users");
            DropForeignKey("dbo.TransactionTemplates", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.TransactionTemplates", "TargetAccountId", "dbo.Accounts");
            DropForeignKey("dbo.TransactionTemplates", "SourceAccountId", "dbo.Accounts");
            DropForeignKey("dbo.TransactionTemplates", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.TransactionTemplates", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.TransactionTemplates", "BudgetId", "dbo.Budgets");
            DropIndex("dbo.TransactionTemplates", new[] { "UpdateUserId" });
            DropIndex("dbo.TransactionTemplates", new[] { "InsertUserId" });
            DropIndex("dbo.TransactionTemplates", new[] { "TargetAccountId" });
            DropIndex("dbo.TransactionTemplates", new[] { "SourceAccountId" });
            DropIndex("dbo.TransactionTemplates", new[] { "CategoryId" });
            DropIndex("dbo.TransactionTemplates", "IX_TemplateName_User_Budget");
            DropTable("dbo.TransactionTemplates");
        }
    }
}
