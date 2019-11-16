using ExpenseTracker.Persistence.Context.DbModels;
using ExpenseTracker.Persistence.Context.FluentConfiguration;
using ExpenseTracker.Persistence.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context
{
    public class ExpenseTrackerContext : IdentityDbContext<User>
    {
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountType> AccountTypes { get; set; }
        public virtual DbSet<BudgetPlanCategory> BudgetPlanCategories { get; set; }
        public virtual DbSet<BudgetPlan> BudgetPlans { get; set; }
        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<BudgetUser> BudgetUsers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<TransactionTemplate> TransactionTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AuditableEntityConfiguration.Configure(modelBuilder);

            AccountConfiguration.Configure(modelBuilder);

            BudgetConfiguration.Configure(modelBuilder);

            BudgetUserConfiguration.Configure(modelBuilder);

            CategoryConfiguration.Configure(modelBuilder);

            IdentityConfiguration.Configure(modelBuilder);

            TransactionTemplateConfiguration.Configure(modelBuilder);
        }

        public static ExpenseTrackerContext Create() => new ExpenseTrackerContext();
    }
}
