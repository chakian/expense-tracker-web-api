using ExpenseTracker.Models.UserModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<RegisterUserResponse> RegisterUser(RegisterUserRequest user);
        Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest user);
    }
}
