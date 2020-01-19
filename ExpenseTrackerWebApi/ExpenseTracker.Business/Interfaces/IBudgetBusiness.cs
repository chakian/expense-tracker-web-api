using ExpenseTracker.Models.BudgetModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IBudgetBusiness
    {
        Task<CreateBudgetResponse> CreateBudget(CreateBudgetRequest request);
    }
}
