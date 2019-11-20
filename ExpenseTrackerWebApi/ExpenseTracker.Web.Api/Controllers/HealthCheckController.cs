using ExpenseTracker.Web.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Web.Api.Controllers
{
    //TODO: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks?view=aspnetcore-3.0
    //TODO: https://medium.com/it-dead-inside/implementing-health-checks-in-asp-net-core-a8331d16a180
    [ApiVersionNeutral]
    [ApiController]
    [Route("api/healthcheck")]
    public class HealthCheckController : Controller
    {
        [HttpGet]
        public HealthCheck Get()
        {
            var health = new HealthCheck()
            {
                Alive = true
            };
            var version = HttpContext.GetRequestedApiVersion();
            health.LatestVersion = version.MajorVersion.ToString() + "." + version.MinorVersion.ToString();
            return health;
        }
    }
}
