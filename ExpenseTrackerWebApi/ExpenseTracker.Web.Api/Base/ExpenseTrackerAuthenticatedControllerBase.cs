using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api
{
    [Authorize]
    public class ExpenseTrackerAuthenticatedControllerBase<T> : ExpenseTrackerControllerBase<T>
    {
        public ExpenseTrackerAuthenticatedControllerBase(ILogger<T> logger) : base(logger)
        {
        }
    }
}
