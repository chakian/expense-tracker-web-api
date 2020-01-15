namespace ExpenseTracker.Models.Base
{
    public class BaseAuthenticatedRequest : BaseRequest
    {
        public string Token { get; set; }
    }
}
