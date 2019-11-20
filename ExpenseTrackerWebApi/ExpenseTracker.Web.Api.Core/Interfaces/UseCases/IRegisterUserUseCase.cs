using ExpenseTracker.Web.Api.Core.Dto.UseCaseRequests;
using ExpenseTracker.Web.Api.Core.Dto.UseCaseResponses;

namespace ExpenseTracker.Web.Api.Core.Interfaces.UseCases
{
    public interface IRegisterUserUseCase : IUseCaseRequestHandler<RegisterUserRequest, RegisterUserResponse>
    {
    }
}
