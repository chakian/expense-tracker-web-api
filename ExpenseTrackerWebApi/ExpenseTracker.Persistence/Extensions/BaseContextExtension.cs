using ExpenseTracker.Persistence.Context.DbModels;
using System.Data.Entity;
using System.Linq;

namespace Persistence.Extensions
{
    public static class BaseContextExtension
    {
        public static IQueryable<T> GetAllActiveAsQueryable<T>(this DbSet<T> model)
            where T : BaseDbo
        {
            return model.Where(q => q.IsActive);
        }
    }
}
