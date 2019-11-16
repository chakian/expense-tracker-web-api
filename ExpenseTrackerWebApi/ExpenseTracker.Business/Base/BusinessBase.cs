using ExpenseTracker.Persistence.Context;

namespace ExpenseTracker.Business
{
    public class BusinessBase
    {
        protected readonly ExpenseTrackerContext context;

        public BusinessBase(ExpenseTrackerContext context)
        {
            this.context = context;
        }
    }
}
