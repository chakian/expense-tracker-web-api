using ExpenseTracker.Common.Interfaces.Models;

namespace ExpenseTracker.Models.Base
{
    public class BaseAuthenticatedRequest : BaseRequest, IBaseAuthenticatedRequest
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
