using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Persistence.Context.DbModels
{
    public class UserInternalToken
    {
        public UserInternalToken()
        {
            Id = new Guid().ToString();
        }

        public string Id { get; set; }

        [Required]
        public string TokenString { get; set; }

        [Required]
        public string UserId { get; set; }
        
        [Required]
        public string Issuer { get; set; }

        public string CreatingIp { get; set; }

        public string Device { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public DateTime LastUsedDate { get; set; }

        public bool IsValid { get; set; }
    }
}
