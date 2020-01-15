using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Persistence.Context.FluentConfiguration
{
    public class BudgetPlanCategoryConfiguration
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BudgetPlanCategory>().Property(p => p.PlannedAmount)
                .HasColumnType("Money");
        }
    }
}
