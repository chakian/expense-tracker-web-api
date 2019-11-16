using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class AuditableEntityConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            Configure<Account>(modelBuilder);
            Configure<Budget>(modelBuilder);
            Configure<BudgetPlan>(modelBuilder);
            Configure<BudgetPlanCategory>(modelBuilder);
            Configure<BudgetUser>(modelBuilder);
            Configure<Category>(modelBuilder);
            Configure<Transaction>(modelBuilder);
            Configure<TransactionTemplate>(modelBuilder);
        }

        private static void Configure<T>(ModelBuilder modelBuilder)
            where T : AuditableDbo
        {
            modelBuilder.Entity<T>()
                .HasOne(s => s.InsertUser)
                .WithMany()
                .HasForeignKey(e => e.InsertUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<T>()
                .HasOne(s => s.UpdateUser)
                .WithMany()
                .HasForeignKey(e => e.UpdateUserId);
        }
    }
}
