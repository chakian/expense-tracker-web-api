using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class UpdateBudgetResponse : BaseAuthenticatedResponse
    {
        public int BudgetId { get; set; }
    }
}
