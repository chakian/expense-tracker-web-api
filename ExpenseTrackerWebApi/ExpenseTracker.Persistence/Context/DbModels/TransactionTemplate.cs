using ExpenseTracker.Persistence.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class TransactionTemplate : AuditableDbo
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(250)]
        [Index("IX_TemplateName_User_Budget", 0, IsUnique = true)]
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int? SourceAccountId { get; set; }
        public virtual Account SourceAccount { get; set; }

        public int? TargetAccountId { get; set; }
        public virtual Account TargetAccount { get; set; }

        [Index("IX_TemplateName_User_Budget", 1, IsUnique = true)]
        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        [Index("IX_TemplateName_User_Budget", 2, IsUnique = true)]
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
