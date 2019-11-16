using ExpenseTracker.Persistence.Context.DbModels;
using System.Data.Entity;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class TransactionTemplateConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TransactionTemplate>()
                .HasOptional(t => t.SourceAccount)
                .WithMany()
                .HasForeignKey(t => t.SourceAccountId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TransactionTemplate>()
                .HasOptional(t => t.TargetAccount)
                .WithMany()
                .HasForeignKey(t => t.TargetAccountId)
                .WillCascadeOnDelete(false);
        }
    }
}
