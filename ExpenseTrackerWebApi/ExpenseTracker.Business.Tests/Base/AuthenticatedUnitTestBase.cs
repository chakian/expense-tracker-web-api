using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.Base
{
    public class AuthenticatedUnitTestBase : UnitTestBase
    {
        public AuthenticatedUnitTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected string AddUser()
        {
            return "";
        }
    }
}
