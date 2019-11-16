﻿using ExpenseTracker.Business;
using ExpenseTracker.Web.Api.Models.ResponseModels;
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
        public BudgetController(ILogger<BudgetController> logger) : base(logger)
        {
        }

        [HttpGet]
        public IEnumerable<Budget> GetAll()
        {
            BudgetBusiness budgetBusiness = new BudgetBusiness();
            budgetBusiness.GetBudgetListOfUser();
            return null;
            //ExpenseTrackerContext
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = rng.Next(-20, 55),
            //    Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}