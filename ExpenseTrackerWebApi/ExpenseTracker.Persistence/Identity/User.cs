using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Persistence.Identity
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public bool IsActive { get; set; }

        public string InsertUserId { get; set; }
        public User InsertUser { get; set; }
        public DateTime InsertTime { get; set; }

        public string UpdateUserId { get; set; }
        public User UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }

        public int? ActiveBudgetId { get; set; }
    }
}
