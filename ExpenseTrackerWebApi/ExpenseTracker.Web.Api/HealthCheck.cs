namespace ExpenseTracker.Web.Api
{
    public class HealthCheck
    {
        public bool Alive { get; set; }
        public string LatestVersion { get; set; }
    }
}
