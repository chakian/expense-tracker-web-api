using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Models.BudgetModels;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.BudgetTests
{
    public class CreateBudgetTests : BudgetUnitTestBase
    {
        public CreateBudgetTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        private BudgetBusiness GetBudgetBusiness()
        {
            return new BudgetBusiness(GetLogger<BudgetBusiness>(), DbContext);
        }

        [Fact]
        public void CreateBudget_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            CreateBudgetRequest request = new CreateBudgetRequest()
            {
                UserId = user.Id,
                BudgetName = "testBudget",
                CurrencyId = currency.CurrencyId
            };
            var expected = new CreateBudgetResponse()
            {
                BudgetId = 1
            };

            // Act
            var actual = GetBudgetBusiness().CreateBudget(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.BudgetId, actual.BudgetId);
        }

        [Fact]
        public void CreateBudget_Fail_SameNameExists()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();

            var budget = new Persistence.Context.DbModels.Budget()
            {
                Name = "testBudget",
                CurrencyId = currency.CurrencyId,
                IsActive = true
            };
            DbContext.Budgets.Add(budget);
            DbContext.SaveChanges();
            DbContext.BudgetUsers.Add(new Persistence.Context.DbModels.BudgetUser()
            {
                UserId = user.Id,
                BudgetId = budget.BudgetId
            });
            DbContext.SaveChanges();

            CreateBudgetRequest request = new CreateBudgetRequest()
            {
                UserId = user.Id,
                BudgetName = "testBudget",
                CurrencyId = currency.CurrencyId
            };
            var expected = new CreateBudgetResponse()
            {
                BudgetId = 1
            };

            // Act
            var actual = GetBudgetBusiness().CreateBudget(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.BudgetId, actual.BudgetId);
        }
    }
}
