namespace ExpenseTracker.Web.Api.Models
{
    public class HealthCheck
    {
        public bool Alive { get; set; }
        public string LatestVersion { get; set; }
    }
}
