using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ExpenseTracker.Persistence.Identity
{
    // Configure the application sign-in manager which is used in this application.
    public class ExpenseSignInManager : SignInManager<User, string>
    {
        public ExpenseSignInManager(ExpenseUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ExpenseUserManager)UserManager);
        }

        public static ExpenseSignInManager Create(IdentityFactoryOptions<ExpenseSignInManager> options, IOwinContext context)
        {
            return new ExpenseSignInManager(context.GetUserManager<ExpenseUserManager>(), context.Authentication);
        }
    }
}
