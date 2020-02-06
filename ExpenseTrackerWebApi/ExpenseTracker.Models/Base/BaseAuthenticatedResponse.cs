using ExpenseTracker.Common.Interfaces.Models;

namespace ExpenseTracker.Models.Base
{
    public class BaseAuthenticatedResponse : BaseResponse, IBaseAuthenticatedResponse
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
