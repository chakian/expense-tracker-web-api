using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Persistence.Context.DbModels;
//using Microsoft.EntityFrameworkCore.Relational;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExpenseTracker.Business
{
    public class BudgetBusiness : AuthenticatedBusinessBase<BudgetBusiness>, IBudgetBusiness
    {
        private readonly ExpenseTrackerContext dbContext;

        public BudgetBusiness(ILogger<BudgetBusiness> logger, ExpenseTrackerContext dbContext, IBudgetUserBusiness budgetUserBusiness) : base(logger)
        {
            this.dbContext = dbContext;
        }

        public async Task<CreateBudgetResponse> CreateBudget(CreateBudgetRequest request)
        {
            CreateBudgetResponse response = new CreateBudgetResponse();

            //dbContext.Database.BeginTransaction(IsolationLevel.Snapshot);
            dbContext.Database.BeginTransaction();

            Budget budget = new Budget()
            {
            };
            await dbContext.Budgets.AddAsync(budget);
            dbContext.SaveChanges();

            BudgetUser budgetUser = new BudgetUser()
            {
            };
            dbContext.BudgetUsers.Add(budgetUser);

            if (response.IsSuccessful == false)
            {
                dbContext.Database.CommitTransaction();
            }
            else
            {
                dbContext.Database.RollbackTransaction();
            }

            return response;
        }
    }
}
