using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class CreateBudgetResponse : BaseAuthenticatedResponse
    {
        public int BudgetId { get; set; }
    }
}
