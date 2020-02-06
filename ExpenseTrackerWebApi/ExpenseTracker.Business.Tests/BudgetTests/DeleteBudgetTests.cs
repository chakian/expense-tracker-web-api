using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Common.Constants;
using ExpenseTracker.Models.BudgetModels;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.BudgetTests
{
    public class DeleteBudgetTests : BudgetUnitTestBase
    {
        public DeleteBudgetTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void DeleteBudget_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);

            var request = new DeleteBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user.Id,
            };

            var budgetsUsers = DbContext.BudgetUsers.Where(b => b.BudgetId == budget.BudgetId).ToList();
            Assert.NotEmpty(budgetsUsers);

            // Act
            var actual = GetBudgetBusiness().DeleteBudget(request).Result;

            // Assert
            AssertSuccessCase(actual);

            var deletedBudget = DbContext.Budgets.SingleOrDefault(b => b.BudgetId == budget.BudgetId);
            Assert.Null(deletedBudget);

            var deletedBudgetsUsers = DbContext.BudgetUsers.Where(b => b.BudgetId == budget.BudgetId).ToList();
            Assert.Empty(deletedBudgetsUsers);
        }

        [Fact]
        public void DeleteBudget_Fail_BudgetDoesntBelongToModifyingUser()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            AddBudget("testBudget", currency.CurrencyId, user.Id);
            var budget2 = AddBudget("testBudget2", currency.CurrencyId, user2.Id);

            var request = new DeleteBudgetRequest()
            {
                BudgetId = budget2.BudgetId,
                UserId = user.Id,
            };

            // Act
            var actual = GetBudgetBusiness().DeleteBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_DOESNT_BELONG_TO_MODIFYING_USER);
        }

        [Fact]
        public void DeleteBudget_Fail_UserIsNotAuthorizedToDelete()
        {
            // Arrange
            var user = AddUser();
            var user2 = AddUser("test2", "test2@email.com");
            var currency = AddCurrency();
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);
            AddUserToBudget(budget.BudgetId, user2.Id, false, true, true, true);

            var request = new DeleteBudgetRequest()
            {
                BudgetId = budget.BudgetId,
                UserId = user2.Id,
            };

            // Act
            var actual = GetBudgetBusiness().DeleteBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_USER_IS_NOT_AUTHORIZED_TO_DELETE_BUDGET);
        }
    }
}
