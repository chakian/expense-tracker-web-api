﻿using ExpenseTracker.Business.Tests.Base;
using ExpenseTracker.Common.Constants;
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

        [Fact]
        public void CreateBudget_Success()
        {
            // Arrange
            var user = AddUser();
            var currency = AddCurrency();
            var request = new CreateBudgetRequest()
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
            var budget = AddBudget("testBudget", currency.CurrencyId, user.Id);

            var request = new CreateBudgetRequest()
            {
                UserId = user.Id,
                BudgetName = budget.Name,
                CurrencyId = currency.CurrencyId
            };

            // Act
            var actual = GetBudgetBusiness().CreateBudget(request).Result;

            // Assert
            AssertSingleErrorCase(actual, ErrorCodes.BUDGET_EXISTS_WITH_SAME_NAME);
        }
    }
}
