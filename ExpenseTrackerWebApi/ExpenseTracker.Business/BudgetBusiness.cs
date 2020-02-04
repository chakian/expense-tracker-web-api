using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Business
{
    public class BudgetBusiness : BudgetBusinessBase<BudgetBusiness>, IBudgetBusiness
    {
        public BudgetBusiness(ILogger<BudgetBusiness> logger, ExpenseTrackerContext dbContext) : base(logger, dbContext)
        {
        }

        #region Private Methods
        private bool DoesBudgetExistsWithSameNameForUser(string budgetName, string userId)
        {
            var budgetList = GetBudgetsOfUser(userId);
            return DoesBudgetExistsWithSameName(budgetList, budgetName);
        }

        private bool DoesBudgetExistsWithSameNameForOtherUsersOfThisBudget(int budgetId, string budgetName, string userId)
        {
            var userList = GetUsersOfBudget(budgetId);
            foreach (var user in userList)
            {
                if (DoesBudgetExistsWithSameNameForUser(budgetName, user.UserId))
                {
                    return true;
                }
            }
            return false;
        }

        private bool DoesBudgetExistsWithSameName(List<Budget> budgetList, string budgetName)
        {
            if (budgetList.Any(b => b.IsActive && b.Name.ToLowerInvariant().Equals(budgetName.ToLowerInvariant())))
            {
                return true;
            }

            return false;
        }
        #endregion

        public async Task<CreateBudgetResponse> CreateBudget(CreateBudgetRequest request)
        {
            CreateBudgetResponse response = new CreateBudgetResponse();

            if (DoesBudgetExistsWithSameNameForUser(request.BudgetName, request.UserId))
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

        public async Task<UpdateBudgetResponse> UpdateBudget(UpdateBudgetRequest request)
        {
            UpdateBudgetResponse response = new UpdateBudgetResponse();

            var budget = GetBudgetById(request.BudgetId, request.UserId);
            if(budget == null)
            {
                response.AddError(ErrorCodes.BUDGET_DOESNT_BELONG_TO_MODIFYING_USER);
                return response;
            }
            if (DoesBudgetExistsWithSameNameForUser(request.BudgetName, request.UserId))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
                return response;
            }
            if (DoesBudgetExistsWithSameNameForOtherUsersOfThisBudget(request.BudgetId, request.BudgetName, request.UserId))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_ON_ANOTHER_USER_WITH_SAME_NAME);
                return response;
            }

            if(!string.IsNullOrEmpty(request.BudgetName))
            {
                budget.Name = request.BudgetName;
            }
            if (request.CurrencyId > 0)
            {
                budget.CurrencyId = request.CurrencyId;
            }
            await dbContext.SaveChangesAsync();

            response.BudgetId = budget.BudgetId;

            return response;
        }
        
        public async Task<DeleteBudgetResponse> DeleteBudget(DeleteBudgetRequest request) => throw new NotImplementedException();
        
        public async Task<GetBudgetsResponse> GetBudgets(GetBudgetsRequest request) => throw new NotImplementedException();
    }
}
