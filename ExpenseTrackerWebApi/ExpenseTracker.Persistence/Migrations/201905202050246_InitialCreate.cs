namespace ExpenseTracker.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        StartingBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CurrentBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AccountTypeId = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.AccountTypes", t => t.AccountTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Budgets", t => t.BudgetId)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .Index(t => t.AccountTypeId)
                .Index(t => t.BudgetId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.AccountTypes",
                c => new
                    {
                        AccountTypeId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AccountTypeId);
            
            CreateTable(
                "dbo.Budgets",
                c => new
                    {
                        BudgetId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CurrencyId = c.Int(nullable: false),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetId)
                .ForeignKey("dbo.Currencies", t => t.CurrencyId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .Index(t => t.CurrencyId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.BudgetPlans",
                c => new
                    {
                        BudgetPlanId = c.Int(nullable: false, identity: true),
                        Month = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetPlanId)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .Index(t => t.BudgetId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.BudgetPlanCategories",
                c => new
                    {
                        BudgetPlanCategoryId = c.Int(nullable: false, identity: true),
                        PlannedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BudgetPlanId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetPlanCategoryId)
                .ForeignKey("dbo.BudgetPlans", t => t.BudgetPlanId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .Index(t => t.BudgetPlanId)
                .Index(t => t.CategoryId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        BudgetId = c.Int(nullable: false),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .ForeignKey("dbo.Budgets", t => t.BudgetId)
                .Index(t => t.BudgetId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        IsActive = c.Boolean(nullable: false),
                        InsertUserId = c.String(maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        ActiveBudgetId = c.Int(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        TransactionId = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        CategoryId = c.Int(nullable: false),
                        SourceAccountId = c.Int(nullable: false),
                        TargetAccountId = c.Int(),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TransactionId)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .ForeignKey("dbo.Accounts", t => t.SourceAccountId, cascadeDelete: true)
                .ForeignKey("dbo.Accounts", t => t.TargetAccountId)
                .Index(t => t.CategoryId)
                .Index(t => t.SourceAccountId)
                .Index(t => t.TargetAccountId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.BudgetUsers",
                c => new
                    {
                        BudgetUserId = c.Int(nullable: false, identity: true),
                        BudgetId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        InsertUserId = c.String(nullable: false, maxLength: 128),
                        InsertTime = c.DateTime(nullable: false),
                        UpdateUserId = c.String(maxLength: 128),
                        UpdateTime = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.BudgetUserId)
                .ForeignKey("dbo.Budgets", t => t.BudgetId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.InsertUserId)
                .ForeignKey("dbo.Users", t => t.UpdateUserId)
                .ForeignKey("dbo.Users", t => t.UserId)
                .Index(t => t.BudgetId)
                .Index(t => t.UserId)
                .Index(t => t.InsertUserId)
                .Index(t => t.UpdateUserId);
            
            CreateTable(
                "dbo.Currencies",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false),
                        CurrencyCode = c.String(nullable: false, maxLength: 15),
                        LongName = c.String(nullable: false, maxLength: 150),
                        DisplayName = c.String(nullable: false, maxLength: 15),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CurrencyId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Accounts", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.Transactions", "TargetAccountId", "dbo.Accounts");
            DropForeignKey("dbo.Transactions", "SourceAccountId", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.Budgets", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.Budgets", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.Budgets", "CurrencyId", "dbo.Currencies");
            DropForeignKey("dbo.Categories", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.BudgetUsers", "UserId", "dbo.Users");
            DropForeignKey("dbo.BudgetUsers", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.BudgetUsers", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.BudgetUsers", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.BudgetPlans", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.BudgetPlans", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.BudgetPlanCategories", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.BudgetPlanCategories", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.Categories", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.Transactions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Transactions", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.Transactions", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.Categories", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.Users", "UpdateUserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "InsertUserId", "dbo.Users");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.BudgetPlanCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.BudgetPlanCategories", "BudgetPlanId", "dbo.BudgetPlans");
            DropForeignKey("dbo.BudgetPlans", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Accounts", "BudgetId", "dbo.Budgets");
            DropForeignKey("dbo.Accounts", "AccountTypeId", "dbo.AccountTypes");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.BudgetUsers", new[] { "UpdateUserId" });
            DropIndex("dbo.BudgetUsers", new[] { "InsertUserId" });
            DropIndex("dbo.BudgetUsers", new[] { "UserId" });
            DropIndex("dbo.BudgetUsers", new[] { "BudgetId" });
            DropIndex("dbo.Transactions", new[] { "UpdateUserId" });
            DropIndex("dbo.Transactions", new[] { "InsertUserId" });
            DropIndex("dbo.Transactions", new[] { "TargetAccountId" });
            DropIndex("dbo.Transactions", new[] { "SourceAccountId" });
            DropIndex("dbo.Transactions", new[] { "CategoryId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.Users", new[] { "UpdateUserId" });
            DropIndex("dbo.Users", new[] { "InsertUserId" });
            DropIndex("dbo.Categories", new[] { "UpdateUserId" });
            DropIndex("dbo.Categories", new[] { "InsertUserId" });
            DropIndex("dbo.Categories", new[] { "BudgetId" });
            DropIndex("dbo.BudgetPlanCategories", new[] { "UpdateUserId" });
            DropIndex("dbo.BudgetPlanCategories", new[] { "InsertUserId" });
            DropIndex("dbo.BudgetPlanCategories", new[] { "CategoryId" });
            DropIndex("dbo.BudgetPlanCategories", new[] { "BudgetPlanId" });
            DropIndex("dbo.BudgetPlans", new[] { "UpdateUserId" });
            DropIndex("dbo.BudgetPlans", new[] { "InsertUserId" });
            DropIndex("dbo.BudgetPlans", new[] { "BudgetId" });
            DropIndex("dbo.Budgets", new[] { "UpdateUserId" });
            DropIndex("dbo.Budgets", new[] { "InsertUserId" });
            DropIndex("dbo.Budgets", new[] { "CurrencyId" });
            DropIndex("dbo.Accounts", new[] { "UpdateUserId" });
            DropIndex("dbo.Accounts", new[] { "InsertUserId" });
            DropIndex("dbo.Accounts", new[] { "BudgetId" });
            DropIndex("dbo.Accounts", new[] { "AccountTypeId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Currencies");
            DropTable("dbo.BudgetUsers");
            DropTable("dbo.Transactions");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.Categories");
            DropTable("dbo.BudgetPlanCategories");
            DropTable("dbo.BudgetPlans");
            DropTable("dbo.Budgets");
            DropTable("dbo.AccountTypes");
            DropTable("dbo.Accounts");
        }
    }
}
