using ExpenseTracker.Persistence.Context;
using System.Linq;

namespace ExpenseTracker.Business
{
    public class BudgetBusiness : BusinessBase
    {
        public BudgetBusiness(ExpenseTrackerContext context)
            : base(context)
        {
        }

        public void GetBudgetListOfUser()
        {
            var x = context.Budgets.ToList();
        }
    }
}
