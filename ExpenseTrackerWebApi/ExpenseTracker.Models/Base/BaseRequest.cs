using ExpenseTracker.Common.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models.Base
{
    public class BaseRequest : IBaseRequest
    {
        [Required]
        public string RequestIp { get; set; }
    }
}
