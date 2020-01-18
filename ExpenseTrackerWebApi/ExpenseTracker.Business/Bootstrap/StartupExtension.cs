using ExpenseTracker.Common.Interfaces.DbContext;
using ExpenseTracker.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace ExpenseTracker.Business.Bootstrap
{
    [ExcludeFromCodeCoverage]
    public static class StartupExtension
    {
        public static void AddExpenseTrackerDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ExpenseTrackerContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddScopedExpenseTrackerDbContext(this IServiceCollection services)
        {
            services.AddScoped<IDbContext, ExpenseTrackerContext>();
        }
    }
}
