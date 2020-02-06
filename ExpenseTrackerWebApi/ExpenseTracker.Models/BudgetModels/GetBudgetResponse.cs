using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class GetBudgetResponse : BaseAuthenticatedResponse
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public bool CanDeleteBudget { get; set; }
        public bool CanModifyBudget { get; set; }
    }
}
