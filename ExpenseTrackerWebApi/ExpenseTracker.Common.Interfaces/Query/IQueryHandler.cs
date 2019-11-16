using System.Threading.Tasks;

namespace ExpenseTracker.Common.Interfaces.Query
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
