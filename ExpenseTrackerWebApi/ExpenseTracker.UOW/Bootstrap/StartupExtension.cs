using ExpenseTracker.Business;
using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.UOW.Bootstrap
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExpenseTrackerContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbContext, ExpenseTrackerContext>();
        }

        public static void AddBusinessImplementationsToScope(this IServiceCollection services)
        {
            services.AddScoped<IUserBusiness, UserBusiness>();
            services.AddScoped<IUserInternalTokenBusiness, UserInternalTokenBusiness>();
        }
    }
}
