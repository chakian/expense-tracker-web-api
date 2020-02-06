using ExpenseTracker.Persistence.Context.DbModels;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.Base
{
    public class BudgetUnitTestBase : AuthenticatedUnitTestBase
    {
        public BudgetUnitTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        protected BudgetBusiness GetBudgetBusiness()
        {
            return new BudgetBusiness(GetLogger<BudgetBusiness>(), DbContext);
        }

        protected Currency AddCurrency(string currencyCode = "TRY", string displayName = "TL", string longName = "TURKISH_LIRA")
        {
            Currency currency = new Currency()
            {
                CurrencyCode = currencyCode,
                DisplayName = displayName,
                LongName = longName,
                IsActive = true
            };
            DbContext.Currencies.Add(currency);
            DbContext.SaveChanges();
            return currency;
        }

        protected Budget AddBudget(string budgetName, int currencyId, string userId)
        {
            var budget = new Budget()
            {
                Name = budgetName,
                CurrencyId = currencyId,
                IsActive = true
            };
            DbContext.Budgets.Add(budget);
            DbContext.SaveChanges();
            DbContext.BudgetUsers.Add(new Persistence.Context.DbModels.BudgetUser()
            {
                UserId = userId,
                BudgetId = budget.BudgetId,
                IsActive = true,
                IsOwner = true,
                IsAdmin = true,
                CanRead = true,
                CanWrite = true,
                CanDelete = true
            });
            DbContext.SaveChanges();

            return budget;
        }

        protected void AddUserToBudget(int budgetId, string userId, bool isAdmin, bool canRead, bool canWrite, bool canDelete)
        {
            DbContext.BudgetUsers.Add(new Persistence.Context.DbModels.BudgetUser()
            {
                UserId = userId,
                BudgetId = budgetId,
                IsActive = true,
                IsOwner = false,
                IsAdmin = isAdmin,
                CanRead = canRead,
                CanWrite = canWrite,
                CanDelete = canDelete
            });
            DbContext.SaveChanges();
        }
    }
}
