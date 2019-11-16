using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class BudgetConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Budget>()
                .HasMany(e => e.Accounts)
                .WithOne(e => e.Budget)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Budget>()
                .HasMany(e => e.Categories)
                .WithOne(e => e.Budget)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
