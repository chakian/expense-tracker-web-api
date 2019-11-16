using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class AccountConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasMany(e => e.TransactionsBySourceAccount)
                .WithOne(e=>e.SourceAccount)
                .HasForeignKey(e => e.SourceAccountId);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.TransactionsByTargetAccount)
                .WithOne(e=>e.TargetAccount)
                .IsRequired(false)
                .HasForeignKey(e => e.TargetAccountId);
        }
    }
}
