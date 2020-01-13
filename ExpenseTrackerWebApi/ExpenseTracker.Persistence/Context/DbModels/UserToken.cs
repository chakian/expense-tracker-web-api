using System;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class UserToken
    {
        public string Id { get; set; }
        
        public string Issuer { get; set; }
        
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public DateTime LastUsedDate { get; set; }

        public string LastLoginIp { get; set; }

        public bool IsValid { get; set; }
    }
}
