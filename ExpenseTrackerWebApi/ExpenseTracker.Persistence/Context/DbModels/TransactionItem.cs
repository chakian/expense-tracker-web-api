using System;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class TransactionItem
    {
        public int TransactionItemId { get; set; }

        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }

        public DateTime Date { get; set; }

        public decimal Amount { get; set; }

        public string Description { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
