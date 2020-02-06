using ExpenseTracker.Models.Base;

namespace ExpenseTracker.Models.UserModels
{
    public sealed class AuthenticateUserResponse : BaseResponse
    {
        public string Id { get; set; }

        public string Token { get; set; }

        public string Name { get; set; }

        public string Culture { get; set; }

        //public virtual string Email { get; set; }
        //public bool IsActive { get; set; }
        //public int? ActiveBudgetId { get; set; }
        
        //public virtual DateTimeOffset? LockoutEnd { get; set; }
        //public virtual bool TwoFactorEnabled { get; set; }
        //public virtual bool PhoneNumberConfirmed { get; set; }
        //public virtual string PhoneNumber { get; set; }
        //public virtual bool EmailConfirmed { get; set; }
        //public virtual string NormalizedEmail { get; set; }
        //public virtual string NormalizedUserName { get; set; }
    }
}
