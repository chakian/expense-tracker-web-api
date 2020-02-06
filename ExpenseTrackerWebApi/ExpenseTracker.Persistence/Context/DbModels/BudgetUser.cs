using ExpenseTracker.Persistence.Identity;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class BudgetUser : AuditableDbo
    {
        public int BudgetUserId { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanDelete { get; set; }
    }
}
