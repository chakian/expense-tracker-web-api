using ExpenseTracker.Persistence.Identity;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class TransactionTemplate : AuditableDbo
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? SourceAccountId { get; set; }
        public virtual Account SourceAccount { get; set; }

        public int? TargetAccountId { get; set; }
        public virtual Account TargetAccount { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
