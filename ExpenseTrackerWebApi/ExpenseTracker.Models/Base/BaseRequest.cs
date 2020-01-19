using ExpenseTracker.Common.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.Base
{
    public class BaseRequest : IBaseRequest
    {
        [Required]
        public string RequestIp { get; set; }

        [RegularExpression("[A-Z]{2}-[a-z]{2}")]
        public string Culture { get; set; }
    }
}
