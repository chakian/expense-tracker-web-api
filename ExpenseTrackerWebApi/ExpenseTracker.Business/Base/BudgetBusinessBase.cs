using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Business.Base
{
    public class BudgetBusinessBase<T> : AuthenticatedBusinessBase<T>
    {
        protected readonly ExpenseTrackerContext dbContext;

        public BudgetBusinessBase(ILogger<T> logger, ExpenseTrackerContext dbContext) : base(logger)
        {
            this.dbContext = dbContext;
        }

        protected List<Budget> GetBudgetsOfUser(string userId) =>
            dbContext.Budgets.Where(b => b.IsActive && b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)))
                .Include(b => b.Currency)
                .ToList();
        protected List<BudgetUser> GetUsersOfBudget(int budgetId) =>
            dbContext.BudgetUsers.Where(bu => bu.IsActive && bu.BudgetId == budgetId)
            .ToList();
        protected Budget GetBudgetById(int budgetId, string userId) =>
            dbContext.Budgets.SingleOrDefault(b => b.BudgetId == budgetId && b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)));
    }
}
