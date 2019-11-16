using ExpenseTracker.Persistence.Identity;
using System;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class AuditableDbo : BaseDbo
    {
        public string InsertUserId { get; set; }
        public User InsertUser { get; set; }
        public DateTime InsertTime { get; set; }

        public string UpdateUserId { get; set; }
        public User UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}
