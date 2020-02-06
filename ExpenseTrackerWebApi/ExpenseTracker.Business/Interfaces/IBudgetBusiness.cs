using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.Models.BudgetUserModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IBudgetBusiness
    {
        Task<CreateBudgetResponse> CreateBudget(CreateBudgetRequest request);
        Task<UpdateBudgetResponse> UpdateBudget(UpdateBudgetRequest request);
        Task<DeleteBudgetResponse> DeleteBudget(DeleteBudgetRequest request);
        Task<GetBudgetsResponse> GetBudgets(GetBudgetsRequest request);
        Task<CreateBudgetUserResponse> CreateBudgetUser(CreateBudgetUserRequest request);
    }
}
