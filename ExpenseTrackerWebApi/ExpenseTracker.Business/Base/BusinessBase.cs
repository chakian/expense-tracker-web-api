using ExpenseTracker.Models.Base;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Business.Base
{
    public class BusinessBase
    {
    }

    public class BusinessBase<T> : BusinessBase
    {
        protected readonly ILogger<T> logger;

        public BusinessBase(ILogger<T> logger)
        {
            this.logger = logger;
        }
    }
}
