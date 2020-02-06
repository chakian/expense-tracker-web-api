using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Extensions;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.Models.BudgetUserModels;
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
        private async Task<bool> DoesBudgetExistsWithSameNameForUser(string budgetName, string userId)
        {
            var budgetList = await GetActiveBudgetsOfUser(userId);
            return DoesBudgetExistsWithSameName(budgetList, budgetName);
        }

        private async Task<bool> DoesBudgetExistsWithSameNameForOtherUsersOfThisBudget(int budgetId, string budgetName, string userId)
        {
            var userList = await GetUsersOfBudget(budgetId);
            foreach (var user in userList)
            {
                if (await DoesBudgetExistsWithSameNameForUser(budgetName, user.UserId))
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

            if (await DoesBudgetExistsWithSameNameForUser(request.BudgetName, request.UserId))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
                return response;
            }

            dbContext.Database.BeginTransaction();
            try
            {
                Budget budget = new Budget()
                {
                    CurrencyId = request.CurrencyId,
                    Name = request.BudgetName,
                    InsertTime = DateTime.UtcNow,
                    InsertUserId = request.UserId
                };
                await dbContext.Budgets.AddAsync(budget);
                dbContext.SaveChanges();

                BudgetUser budgetUser = new BudgetUser()
                {
                    BudgetId = budget.BudgetId,
                    UserId = request.UserId,
                    IsActive = true,
                    IsOwner = true,
                    IsAdmin = true,
                    CanRead = true,
                    CanWrite = true,
                    CanDelete = true,
                    InsertTime = DateTime.UtcNow,
                    InsertUserId = request.UserId
                };
                await dbContext.BudgetUsers.AddAsync(budgetUser);
                dbContext.SaveChanges();

                dbContext.Database.CommitTransaction();

                response.BudgetId = budget.BudgetId;
            }
            catch (Exception e)
            {
                dbContext.Database.RollbackTransaction();
                throw e;
            }

            return response;
        }

        public async Task<UpdateBudgetResponse> UpdateBudget(UpdateBudgetRequest request)
        {
            UpdateBudgetResponse response = new UpdateBudgetResponse();

            var budget = await GetBudgetById(request.BudgetId, request.UserId);
            if (budget == null)
            {
                response.AddError(ErrorCodes.BUDGET_DOESNT_BELONG_TO_MODIFYING_USER);
                return response;
            }
            if (await DoesBudgetExistsWithSameNameForUser(request.BudgetName, request.UserId))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
                return response;
            }
            if (await DoesBudgetExistsWithSameNameForOtherUsersOfThisBudget(request.BudgetId, request.BudgetName, request.UserId))
            {
                response.AddError(ErrorCodes.BUDGET_EXISTS_ON_ANOTHER_USER_WITH_SAME_NAME);
                return response;
            }
            var budgetUser = dbContext.BudgetUsers.Single(bu => bu.IsActive && bu.BudgetId == request.BudgetId && bu.UserId == request.UserId);
            if (!budgetUser.IsAdmin)
            {
                response.AddError(ErrorCodes.BUDGET_USER_IS_NOT_AUTHORIZED_TO_MODIFY_BUDGET);
                return response;
            }

            if (!string.IsNullOrEmpty(request.BudgetName))
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

        public async Task<DeleteBudgetResponse> DeleteBudget(DeleteBudgetRequest request)
        {
            DeleteBudgetResponse response = new DeleteBudgetResponse();

            var budget = await GetBudgetById(request.BudgetId, request.UserId);
            if (budget == null)
            {
                response.AddError(ErrorCodes.BUDGET_DOESNT_BELONG_TO_MODIFYING_USER);
                return response;
            }

            var budgetUser = dbContext.BudgetUsers.Single(bu => bu.IsActive && bu.BudgetId == request.BudgetId && bu.UserId == request.UserId);
            if (!budgetUser.IsAdmin)
            {
                response.AddError(ErrorCodes.BUDGET_USER_IS_NOT_AUTHORIZED_TO_DELETE_BUDGET);
                return response;
            }

            dbContext.Budgets.Remove(budget);
            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<GetBudgetsResponse> GetBudgets(GetBudgetsRequest request)
        {
            GetBudgetsResponse response = new GetBudgetsResponse();
            response.BudgetList = new List<GetBudgetsResponse.Budget>();

            var list = await GetActiveBudgetsOfUser(request.UserId);
            list.ForEach(async b =>
            {
                var budgetUser = await GetUsersRoleInBudget(b.BudgetId, request.UserId);
                response.BudgetList.Add(new GetBudgetsResponse.Budget
                {
                    Id = b.BudgetId,
                    Name = b.Name,
                    CanDelete = budgetUser.IsOwner,
                    CanModify = budgetUser.IsAdmin
                });
            });

            return response;
        }

        public async Task<GetBudgetResponse> GetBudgetById(GetBudgetRequest request)
        {
            GetBudgetResponse response = new GetBudgetResponse();

            var budget = await GetBudgetById(request.BudgetId, request.UserId);
            if (budget == null)
            {
                response.AddError(ErrorCodes.BUDGET_DOESNT_BELONG_TO_USER);
                return response;
            }

            var budgetUser = await GetUsersRoleInBudget(request.BudgetId, request.UserId);

            response.BudgetId = budget.BudgetId;
            response.BudgetName = budget.Name;
            response.CanDeleteBudget = budgetUser.IsOwner;
            response.CanModifyBudget = budgetUser.IsAdmin;

            return response;
        }

        public async Task<CreateBudgetUserResponse> CreateBudgetUser(CreateBudgetUserRequest request) => throw new NotImplementedException();
    }
}
