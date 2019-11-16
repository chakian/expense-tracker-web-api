namespace ExpenseTracker.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class AddTable_TransactionItems : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] { "CategoryId" });

            CreateTable(
                "dbo.TransactionItems",
                c => new
                {
                    TransactionItemId = c.Int(nullable: false, identity: true),
                    TransactionId = c.Int(nullable: false),
                    Date = c.DateTime(nullable: false),
                    Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Description = c.String(),
                    CategoryId = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.TransactionItemId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Transactions", t => t.TransactionId, cascadeDelete: true)
                .Index(t => t.TransactionId)
                .Index(t => t.CategoryId);

            AlterColumn("dbo.Transactions", "CategoryId", c => c.Int(nullable: true));
            CreateIndex("dbo.Transactions", "CategoryId");
            AddForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories", "CategoryId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.TransactionItems", "TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.TransactionItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.TransactionItems", new[] { "CategoryId" });
            DropIndex("dbo.TransactionItems", new[] { "TransactionId" });
            AlterColumn("dbo.Transactions", "CategoryId", c => c.Int(nullable: false));
            DropTable("dbo.TransactionItems");
            CreateIndex("dbo.Transactions", "CategoryId");
            AddForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories", "CategoryId");
        }
    }
}
