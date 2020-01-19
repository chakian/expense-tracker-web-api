using ExpenseTracker.Models.BudgetUserModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IBudgetUserBusiness
    {
        Task<CreateBudgetUserResponse> CreateBudgetUser(CreateBudgetUserRequest request);
    }
}
