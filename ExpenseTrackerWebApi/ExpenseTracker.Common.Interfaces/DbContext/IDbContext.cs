using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExpenseTracker.Common.Interfaces.DbContext
{
    public interface IDbContext
    {
        DatabaseFacade Database { get; }
    }
}
