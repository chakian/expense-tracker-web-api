using ExpenseTracker.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.UserModels
{
    public class RegisterUserRequest : BaseRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string PasswordRepeat { get; set; }
    }
}
