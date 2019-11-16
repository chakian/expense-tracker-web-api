using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class Account : AuditableDbo
    {
        public Account()
        {
            TransactionsBySourceAccount = new HashSet<Transaction>();
            TransactionsByTargetAccount = new HashSet<Transaction>();
        }

        public int AccountId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal StartingBalance { get; set; }

        public decimal CurrentBalance { get; set; }

        public int AccountTypeId { get; set; }
        public virtual AccountType AccountType { get; set; }

        public int BudgetId { get; set; }
        public virtual Budget Budget { get; set; }

        public virtual ICollection<Transaction> TransactionsBySourceAccount { get; set; }

        public virtual ICollection<Transaction> TransactionsByTargetAccount { get; set; }
    }
}
