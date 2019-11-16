using ExpenseTracker.Persistence.Context.DbModels;
using System.Data.Entity;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class CategoryConfiguration
    {
        public static void Configure(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Transactions)
                .WithRequired(e => e.Category)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasOptional(e => e.ParentCategory)
                .WithMany()
                .HasForeignKey(e => e.ParentCategoryId)
                .WillCascadeOnDelete(false);
        }
    }
}
