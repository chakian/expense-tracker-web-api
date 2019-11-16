using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class BudgetUserConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BudgetUser>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
