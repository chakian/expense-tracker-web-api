using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api
{
    public class ExpenseTrackerControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> _logger;
        
        public ExpenseTrackerControllerBase(ILogger<T> logger)
        {
            _logger = logger;
        }
    }
}
