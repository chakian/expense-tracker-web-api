namespace ExpenseTracker.Common.Interfaces.Models
{
    public interface IBaseAuthenticatedRequest : IBaseRequest
    {
        string Token { get; set; }
        string UserId { get; set; }
    }
}
