using System.Collections.Generic;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class BudgetPlanCategory : AuditableDbo
    {
        public int BudgetPlanCategoryId { get; set; }

        public decimal PlannedAmount { get; set; }

        public int BudgetPlanId { get; set; }
        public virtual BudgetPlan BudgetPlan { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
