using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Business.Base
{
    public class AuthenticatedBusinessBase<T> : BusinessBase<T>
    {
        public AuthenticatedBusinessBase(ILogger<T> logger) : base(logger)
        {
        }
    }
}
