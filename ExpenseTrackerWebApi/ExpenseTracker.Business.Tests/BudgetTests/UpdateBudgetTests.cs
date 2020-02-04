using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.BudgetTests
{
    public class UpdateBudgetTests : BudgetUnitTestBase
    {
        public UpdateBudgetTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void UpdateBudget_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);

            var request = new UpdateBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user.Id,
                BudgetName = "testBudget2",
                CurrencyId = currency.CurrencyId
            };
            var expected = new UpdateBudgetResponse()
            {
                BudgetId = budget.BudgetId
            };

            // Act
            var actual = GetBudgetBusiness().UpdateBudget(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(expected.BudgetId, actual.BudgetId);
        }

        [Fact]
        public void UpdateBudget_Fail_BudgetDoesntBelongToModifyingUser()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            AddBudget("testBudget", currency.CurrencyId, user.Id);
            var budget2 = AddBudget("testBudget2", currency.CurrencyId, user2.Id);

            var request = new UpdateBudgetRequest()
            {
                BudgetId = budget2.BudgetId,
                UserId = user.Id,
                BudgetName = "perfectName"
            };

            // Act
            var actual = GetBudgetBusiness().UpdateBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_DOESNT_BELONG_TO_MODIFYING_USER);
        }

        [Fact]
        public void UpdateBudget_Fail_SameNameExists()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            var budget2 = AddBudget("testBudget2", currency.CurrencyId, user.Id);

            var request = new UpdateBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user.Id,
                BudgetName = budget2.Name,
                CurrencyId = currency.CurrencyId
            };

            // Act
            var actual = GetBudgetBusiness().UpdateBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
        }

        [Fact]
        public void UpdateBudget_Fail_SameNameExistsOnAnotherUser()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            AddUserToBudget(budget.BudgetId, user2.Id);
            var budget2 = AddBudget("testBudget2", currency.CurrencyId, user2.Id);

            var request = new UpdateBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user.Id,
                BudgetName = budget2.Name,
                CurrencyId = currency.CurrencyId
            };

            // Act
            var actual = GetBudgetBusiness().UpdateBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_EXISTS_ON_ANOTHER_USER_WITH_SAME_NAME);
        }
    }
}
