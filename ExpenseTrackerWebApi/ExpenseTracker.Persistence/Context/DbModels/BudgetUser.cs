using ExpenseTracker.Persistence.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class BudgetUser : AuditableDbo
    {
        public int BudgetUserId { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
