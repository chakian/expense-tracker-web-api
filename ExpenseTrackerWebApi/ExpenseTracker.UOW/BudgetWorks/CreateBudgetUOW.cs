using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Common.Interfaces.Models;
using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.UOW.Base;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.UOW.BudgetWorks
{
    public class CreateBudgetUOW : AuthenticatedUnitOfWorkBase<CreateBudgetUOW>
    {
        private readonly IBudgetBusiness budgetBusiness;
        public CreateBudgetUOW(ILogger<CreateBudgetUOW> logger, IDbContext dbContext, IBudgetBusiness budgetBusiness) : base(logger, dbContext)
        {
            this.budgetBusiness = budgetBusiness;
        }

        internal override IBaseAuthenticatedResponse ExecuteInternal(IBaseAuthenticatedRequest request)
        {
            CreateBudgetRequest createBudgetRequest = (CreateBudgetRequest)request;
            
            CreateBudgetResponse createBudgetResponse = budgetBusiness.CreateBudget(createBudgetRequest).Result;

            return createBudgetResponse;
        }
    }
}
