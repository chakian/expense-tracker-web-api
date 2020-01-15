using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class Budget : AuditableDbo
    {
        public Budget()
        {
            Accounts = new HashSet<Account>();
            BudgetPlans = new HashSet<BudgetPlan>();
            BudgetUsers = new HashSet<BudgetUser>();
            Categories = new HashSet<Category>();
        }

        public int BudgetId { get; set; }

        [Required]
        public string Name { get; set; }

        public int CurrencyId { get; set; }
        public virtual Currency Currency { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }

        public virtual ICollection<BudgetPlan> BudgetPlans { get; set; }

        public virtual ICollection<BudgetUser> BudgetUsers { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
