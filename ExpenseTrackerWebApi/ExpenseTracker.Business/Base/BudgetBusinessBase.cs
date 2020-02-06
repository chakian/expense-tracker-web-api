using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Base
{
    public class BudgetBusinessBase<T> : AuthenticatedBusinessBase<T>
    {
        protected readonly ExpenseTrackerContext dbContext;

        public BudgetBusinessBase(ILogger<T> logger, ExpenseTrackerContext dbContext) : base(logger)
        {
            this.dbContext = dbContext;
        }

        protected async Task<List<Budget>> GetAllBudgetsOfUser(string userId) =>
            await dbContext.Budgets.Where(b => b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)))
                .Include(b => b.Currency)
                .ToListAsync();
        protected async Task<List<Budget>> GetActiveBudgetsOfUser(string userId) =>
            await dbContext.Budgets.Where(b => b.IsActive && b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)))
                .Include(b => b.Currency)
                .ToListAsync();
        protected async Task<List<BudgetUser>> GetUsersOfBudget(int budgetId) =>
            await dbContext.BudgetUsers.Where(bu => bu.IsActive && bu.BudgetId == budgetId)
            .ToListAsync();
        protected async Task<Budget> GetBudgetById(int budgetId, string userId) =>
            await dbContext.Budgets.SingleOrDefaultAsync(b => b.BudgetId == budgetId && b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)));
        protected async Task<BudgetUser> GetUsersRoleInBudget(int budgetId, string userId) =>
            await dbContext.BudgetUsers.SingleAsync(bu => bu.IsActive && bu.BudgetId == budgetId && bu.UserId == userId);
    }
}
