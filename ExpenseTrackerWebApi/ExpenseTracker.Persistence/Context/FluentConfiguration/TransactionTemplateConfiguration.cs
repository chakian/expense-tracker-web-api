using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class TransactionTemplateConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionTemplate>()
                .Property(p => p.Name)
                .HasMaxLength(250);

            modelBuilder.Entity<TransactionTemplate>()
                .HasIndex(p => new { p.BudgetId, p.UserId, p.Name })
                .HasName("IX_TemplateName_User_Budget")
                .IsUnique();

            modelBuilder.Entity<TransactionTemplate>().Property(p => p.Amount)
                .HasColumnType("Money");

            modelBuilder.Entity<TransactionTemplate>()
                .HasOne(t => t.SourceAccount)
                .WithMany()
                .HasForeignKey(t => t.SourceAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TransactionTemplate>()
                .HasOne(t => t.TargetAccount)
                .WithMany()
                .HasForeignKey(t => t.TargetAccountId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
