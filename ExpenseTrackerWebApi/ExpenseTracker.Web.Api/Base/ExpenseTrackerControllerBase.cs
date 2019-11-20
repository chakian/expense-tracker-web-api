using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api
{
    public class ExpenseTrackerControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;
        
        public ExpenseTrackerControllerBase(ILogger<T> logger)
        {
            this.logger = logger;
        }
    }
}
