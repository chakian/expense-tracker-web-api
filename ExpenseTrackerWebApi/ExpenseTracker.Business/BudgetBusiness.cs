using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Business
{
    public class BudgetBusiness : AuthenticatedBusinessBase<BudgetBusiness>, IBudgetBusiness
    {
        private readonly ExpenseTrackerContext dbContext;

        public BudgetBusiness(ILogger<BudgetBusiness> logger, ExpenseTrackerContext dbContext) : base(logger)
        {
            this.dbContext = dbContext;
        }

        #region Private Methods
        private List<Budget> GetBudgetsOfUser(string userId) =>
            dbContext.Budgets.Where(b => b.IsActive && b.BudgetUsers.Any(bu => bu.IsActive && bu.UserId.Equals(userId)))
                .Include(b => b.Currency)
                .ToList();

        private bool DoesBudgetExistsWithSameNameForUser(List<Budget> budgets, string budgetName)
        {
            if (budgets.Any(b => b.Name.ToLowerInvariant().Equals(budgetName.ToLowerInvariant())))
            {
                return true;
            }

            return false;
        }
        #endregion

        public async Task<CreateBudgetResponse> CreateBudget(CreateBudgetRequest request)
        {
            CreateBudgetResponse response = new CreateBudgetResponse();

            var budgets = GetBudgetsOfUser(request.UserId);
            if (DoesBudgetExistsWithSameNameForUser(budgets, request.BudgetName))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
                return response;
            }

            Budget budget = new Budget()
            {
                CurrencyId = request.CurrencyId,
                Name = request.BudgetName,
                InsertTime = DateTime.UtcNow,
                InsertUserId = request.UserId
            };
            await dbContext.Budgets.AddAsync(budget);
            dbContext.SaveChanges();

            response.BudgetId = budget.BudgetId;

            return response;
        }
    }
}
