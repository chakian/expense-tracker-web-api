using ExpenseTracker.Persistence.Context.DbModels;
using System.Linq;

namespace Persistence.Extensions
{
    public static class AccountContextExtension
    {
        public static IQueryable<Account> AssureUserBelongsToBudget(this IQueryable<Account> model, string userId, int budgetId)
        {
            return model.Where(q => q.BudgetId == budgetId && q.Budget.BudgetUsers.Any(bu => bu.UserId.Equals(userId)));
        }
    }
}
