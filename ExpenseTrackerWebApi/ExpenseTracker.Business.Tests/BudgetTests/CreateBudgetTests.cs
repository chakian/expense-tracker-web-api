using ExpenseTracker.Business.Interfaces;
using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Models.BudgetModels;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.BudgetTests
{
    public class CreateBudgetTests : AuthenticatedUnitTestBase
    {
        public CreateBudgetTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        //[Fact]
        //public void CreateBudget_Success()
        //{
        //    // Arrange
        //    IBudgetBusiness budgetBusiness = new BudgetBusiness(GetLogger<BudgetBusiness>(), DbContext);
        //    var token = AddUser();
        //    CreateBudgetRequest request = new CreateBudgetRequest()
        //    {
        //        Token = "",
        //        BudgetName = "testBudget",
        //        CurrencyId = 1
        //    };
        //    var expected = new CreateBudgetResponse()
        //    {
        //        BudgetId = 1
        //    };

        //    // Act
        //    var actual = budgetBusiness.CreateBudget(request).Result;

        //    // Assert
        //    AssertSuccessCase(actual);
        //    Assert.Equal(expected.BudgetId, actual.BudgetId);
        //}
    }
}
