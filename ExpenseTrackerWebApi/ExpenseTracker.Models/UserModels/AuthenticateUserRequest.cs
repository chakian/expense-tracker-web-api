using ExpenseTracker.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.UserModels
{
    public class AuthenticateUserRequest : BaseRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
