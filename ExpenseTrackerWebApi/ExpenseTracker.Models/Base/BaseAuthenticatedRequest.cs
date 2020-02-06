namespace ExpenseTracker.Models.Base
{
    public class BaseAuthenticatedRequest : BaseRequest
    {
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
