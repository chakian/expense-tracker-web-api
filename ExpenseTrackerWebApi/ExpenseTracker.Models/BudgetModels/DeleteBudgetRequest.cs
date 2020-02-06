using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class DeleteBudgetRequest : BaseAuthenticatedRequest
    {
        public int BudgetId { get; set; }
    }
}
