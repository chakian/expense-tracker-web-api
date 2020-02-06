using ExpenseTracker.Common.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Web.Api
{
    public abstract class ExpenseTrackerControllerBase<T> : ControllerBase
    {
        protected readonly ILogger<T> logger;
        
        public ExpenseTrackerControllerBase(ILogger<T> logger)
        {
            this.logger = logger;
        }

        public ActionResult GetActionResult(IBaseResponse response)
        {
            if (response == null)
                return StatusCode(500);

            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
    }
}
