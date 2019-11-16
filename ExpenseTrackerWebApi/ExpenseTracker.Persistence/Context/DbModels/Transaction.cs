using System;
using System.Collections.Generic;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class Transaction : AuditableDbo
    {
        public int TransactionId { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int SourceAccountId { get; set; }
        public virtual Account SourceAccount { get; set; }

        public int? TargetAccountId { get; set; }
        public virtual Account TargetAccount { get; set; }

        public virtual ICollection<TransactionItem> TransactionItems { get; set; }
    }
}
