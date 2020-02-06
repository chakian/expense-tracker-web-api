using ExpenseTracker.Models.Base;
using System.Collections.Generic;

namespace ExpenseTracker.Models.BudgetModels
{
    public class GetBudgetsResponse : BaseAuthenticatedResponse
    {
        public List<Budget> BudgetList { get; set; }
        public class Budget
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public bool CanDelete { get; set; }
            public bool CanModify { get; set; }
        }
    }
}
