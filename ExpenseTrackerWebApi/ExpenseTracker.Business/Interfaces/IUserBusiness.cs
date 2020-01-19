using ExpenseTracker.Models.UserModels;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interfaces
{
    public interface IUserBusiness
    {
        Task<CreateUserResponse> CreateUser(CreateUserRequest user);
        Task<AuthenticateUserResponse> AuthenticateUser(AuthenticateUserRequest user);
    }
}
