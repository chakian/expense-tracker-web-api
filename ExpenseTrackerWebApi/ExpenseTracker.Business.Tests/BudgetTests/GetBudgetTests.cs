using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.BudgetTests
{
    public class GetBudgetTests : BudgetUnitTestBase
    {
        public GetBudgetTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void GetBudgets_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            AddBudget("testBudget2", currency.CurrencyId, user.Id);

            var request = new GetBudgetsRequest()
            {
                UserId = user.Id,
            };

            // Act
            var actual = GetBudgetBusiness().GetBudgets(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.NotEmpty(actual.BudgetList);
            Assert.Equal(2, actual.BudgetList.Count);
            
            Assert.Equal(budget.BudgetId, actual.BudgetList[0].Id);
            Assert.False(string.IsNullOrEmpty(actual.BudgetList[0].Name));
            Assert.True(actual.BudgetList[0].CanDelete);
            Assert.True(actual.BudgetList[0].CanModify);
        }

        [Fact]
        public void GetBudgetById_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);

            var request = new GetBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user.Id,
            };

            // Act
            var actual = GetBudgetBusiness().GetBudgetById(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.Equal(request.BudgetId, actual.BudgetId);
            Assert.False(string.IsNullOrEmpty(actual.BudgetName));
            Assert.True(actual.CanDeleteBudget);
            Assert.True(actual.CanModifyBudget);
        }

        [Fact]
        public void GetBudgetById_Success_IsAdmin()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            AddUserToBudget(budget.BudgetId, user2.Id, true, true, true, true);

            var request = new GetBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user2.Id,
            };

            // Act
            var actual = GetBudgetBusiness().GetBudgetById(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.False(string.IsNullOrEmpty(actual.BudgetName));
            Assert.False(actual.CanDeleteBudget);
            Assert.True(actual.CanModifyBudget);
        }

        [Fact]
        public void GetBudgetById_Success_NotAdmin()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            AddUserToBudget(budget.BudgetId, user2.Id, false, true, true, true);

            var request = new GetBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user2.Id,
            };

            // Act
            var actual = GetBudgetBusiness().GetBudgetById(request).Result;

            // Assert
            AssertSuccessCase(actual);
            Assert.False(string.IsNullOrEmpty(actual.BudgetName));
            Assert.False(actual.CanDeleteBudget);
            Assert.False(actual.CanModifyBudget);
        }

        [Fact]
        public void GetBudget_Fail_BudgetDoesntBelongToUser()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            AddBudget("testBudget", currency.CurrencyId, user.Id);
            var budget2 = AddBudget("testBudget2", currency.CurrencyId, user2.Id);

            var request = new GetBudgetRequest()
            {
                BudgetId = budget2.BudgetId,
                UserId = user.Id,
            };

            // Act
            var actual = GetBudgetBusiness().GetBudgetById(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_DOESNT_BELONG_TO_USER);
        }
    }
}
