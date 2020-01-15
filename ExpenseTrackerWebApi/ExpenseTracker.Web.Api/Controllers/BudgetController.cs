using ExpenseTracker.Web.Api.Models.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseTracker.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/budget")]
    public class BudgetController : ExpenseTrackerAuthenticatedControllerBase<BudgetController>
    {
        public BudgetController(ILogger<BudgetController> logger) : base(logger)
        {
        }

        [HttpGet]
        public IEnumerable<Budget> GetAll()
        {
            var x = User;
            //var budgets = context.Budgets.ToList();//.Where(q=>q.BudgetUsers.Any(u=>u.UserId.Equals(""))).ToList();
            List<Budget> budgetList = new List<Budget>();
            //budgets.ForEach(b =>
            //{
            //    budgetList.Add(new Budget
            //    {
            //        Id = b.BudgetId,
            //        Name = b.Name
            //    });
            //});
            return budgetList;
        }
    }
}
