using ExpenseTracker.Web.Api.Core.Dto.UseCaseResponses;
using ExpenseTracker.Web.Api.Core.Interfaces;

namespace ExpenseTracker.Web.Api.Core.Dto.UseCaseRequests
{
    public class RegisterUserRequest : IUseCaseRequest<RegisterUserResponse>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Username { get; }
        public string Password { get; }

        public RegisterUserRequest(string firstName, string lastName, string email, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = password;
        }
    }
}
