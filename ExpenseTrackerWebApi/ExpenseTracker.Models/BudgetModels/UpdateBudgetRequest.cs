using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class UpdateBudgetRequest : BaseAuthenticatedRequest
    {
        public int BudgetId { get; set; }
        public string BudgetName { get; set; }
        public int CurrencyId { get; set; }
    }
}
