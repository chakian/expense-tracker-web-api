using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.BudgetModels
{
    public class CreateBudgetRequest : BaseAuthenticatedRequest
    {
        public string BudgetName { get; set; }
        public int CurrencyId { get; set; }
    }
}
