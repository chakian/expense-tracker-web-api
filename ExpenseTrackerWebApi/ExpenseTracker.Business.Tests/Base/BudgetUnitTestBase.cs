using ExpenseTracker.Persistence.Context.DbModels;
using Xunit.Abstractions;

namespace ExpenseTracker.Business.Tests.Base
{
    public class BudgetUnitTestBase : AuthenticatedUnitTestBase
    {
        public BudgetUnitTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
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
    }
}
