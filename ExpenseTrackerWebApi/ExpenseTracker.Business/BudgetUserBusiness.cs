using System.Threading.Tasks;
using ExpenseTracker.Business.Base;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Models.BudgetUserModels;
using ExpenseTracker.Persistence.Context;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Business
{
    public class BudgetUserBusiness : AuthenticatedBusinessBase<BudgetUserBusiness>, IBudgetUserBusiness
    {
        private readonly ExpenseTrackerContext dbContext;

        public BudgetUserBusiness(ILogger<BudgetUserBusiness> logger, ExpenseTrackerContext dbContext) : base(logger)
        {
            this.dbContext = dbContext;
        }

        public Task<CreateBudgetUserResponse> CreateBudgetUser(CreateBudgetUserRequest request) => throw new System.NotImplementedException();
    }
}
