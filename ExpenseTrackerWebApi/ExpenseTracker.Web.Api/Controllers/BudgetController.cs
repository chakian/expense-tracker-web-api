using ExpenseTracker.Models.BudgetModels;
using ExpenseTracker.UOW.BudgetWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ExpenseTracker.Web.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/budget")]
    public class BudgetController : ExpenseTrackerAuthenticatedControllerBase<BudgetController>
    {
        private readonly CreateBudgetUOW createBudget;
        public BudgetController(ILogger<BudgetController> logger,
            CreateBudgetUOW createBudget) : base(logger)
        {
            this.createBudget = createBudget;
        }

        //[HttpGet]
        //[Route("getall")]
        //public IEnumerable<Budget> GetAll()
        //{
        //    var x = User;
        //    //var budgets = context.Budgets.ToList();//.Where(q=>q.BudgetUsers.Any(u=>u.UserId.Equals(""))).ToList();
        //    List<Budget> budgetList = new List<Budget>();
        //    //budgets.ForEach(b =>
        //    //{
        //    //    budgetList.Add(new Budget
        //    //    {
        //    //        Id = b.BudgetId,
        //    //        Name = b.Name
        //    //    });
        //    //});
        //    return budgetList;
        //}

        [HttpPut]
        [Route("create")]
        public ActionResult Create([FromBody] CreateBudgetRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = createBudget.Execute(model);

            return GetActionResult(response);
        }
    }
}
