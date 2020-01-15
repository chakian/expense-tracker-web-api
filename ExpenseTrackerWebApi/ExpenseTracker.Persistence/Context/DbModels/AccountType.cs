using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class AccountType : BaseDbo
    {
        public AccountType()
        {
            Accounts = new HashSet<Account>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AccountTypeId { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
