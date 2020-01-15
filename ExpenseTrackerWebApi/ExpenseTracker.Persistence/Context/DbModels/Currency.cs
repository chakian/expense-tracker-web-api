using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class Currency : BaseDbo
    {
        public Currency()
        {
            Budgets = new HashSet<Budget>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(15)]
        public string CurrencyCode { get; set; }

        [Required]
        [StringLength(150)]
        public string LongName { get; set; }

        [Required]
        [StringLength(15)]
        public string DisplayName { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
    }
}
