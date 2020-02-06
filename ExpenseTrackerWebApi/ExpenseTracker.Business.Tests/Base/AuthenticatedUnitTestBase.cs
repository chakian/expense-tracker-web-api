using ExpenseTracker.Models.UserModels;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.Base
{
    public class AuthenticatedUnitTestBase : UnitTestBase
    {
        public AuthenticatedUnitTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected CreateUserResponse AddUser(string username = "testusername", string email = "test@email.com", string password = "testpassword")
        {
            Persistence.Identity.User user = new Persistence.Identity.User()
            {
                Email = email
                UserName = username,
                PasswordHash = password
            };
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
            return new CreateUserResponse()
            {
                Id = user.Id,
                Name = user.UserName
            };
        }
    }
}
