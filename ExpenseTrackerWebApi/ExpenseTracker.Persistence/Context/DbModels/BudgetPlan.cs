using System.Collections.Generic;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class BudgetPlan : AuditableDbo
    {
        public BudgetPlan()
        {
            BudgetPlanCategories = new HashSet<BudgetPlanCategory>();
        }

        public int BudgetPlanId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        public virtual ICollection<BudgetPlanCategory> BudgetPlanCategories { get; set; }
    }
}
