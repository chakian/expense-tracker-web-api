using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.UserModels
{
    public class CreateUserResponse : BaseResponse
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Culture { get; set; }
    }
}
