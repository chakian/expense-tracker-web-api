namespace ExpenseTracker.Models.Base
{
    public class BaseAuthenticatedResponse : BaseResponse
    {
        public string Token { get; set; }
    }
}
