using ExpenseTracker.UOW.UserWorks;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Web.Api.Bootstrap
{
    public static class UnitOfWorkExtension
    {
        public static void AddUnitOfWorkImplementationsToScope(this IServiceCollection services)
        {
            services.AddScoped<CreateUserAndReturnTokenUOW>();
            services.AddScoped<AuthenticateUserAndReturnTokenUOW>();
        }
    }
}
