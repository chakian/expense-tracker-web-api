using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api
{
    public class ExpenseTrackerAuthenticatedControllerBase<T> : ExpenseTrackerControllerBase<T>
    {
        public ExpenseTrackerAuthenticatedControllerBase(ILogger<T> logger) : base(logger)
        {
        }
    }
}
