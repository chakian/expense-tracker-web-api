using ExpenseTracker.Persistence.Context;
using ExpenseTracker.Web.Api.Core.Dto.UseCaseRequests;
using ExpenseTracker.Web.Api.Core.Dto.UseCaseResponses;
using ExpenseTracker.Web.Api.Core.Interfaces;
using ExpenseTracker.Web.Api.Core.Interfaces.UseCases;
using System.Threading.Tasks;

namespace ExpenseTracker.Web.Api.Core.UseCases
{
    public sealed class RegisterUserUseCase : IRegisterUserUseCase
    {
        private readonly ExpenseTrackerContext _context;

        public RegisterUserUseCase(ExpenseTrackerContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RegisterUserRequest message, IOutputPort<RegisterUserResponse> outputPort)
        {
            // TODO: This part should be moved to Business Layer later
            //var response = await _userRepository.Create(message.FirstName, message.LastName, message.Email, message.UserName, message.Password);
            RegisterUserResponse
            outputPort.Handle(response.Success ? new RegisterUserResponse(response.Id, true) : new RegisterUserResponse(response.Errors.Select(e => e.Description)));
            return response.Success;
        }
    }
}
