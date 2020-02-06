namespace ExpenseTracker.Common.Interfaces.Models
{
    public interface IBaseAuthenticatedResponse : IBaseResponse
    {
        string Token { get; set; }
    }
}
