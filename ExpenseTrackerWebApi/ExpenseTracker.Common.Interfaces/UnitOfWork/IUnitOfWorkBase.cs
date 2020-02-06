using ExpenseTracker.Common.Interfaces.Models;

namespace ExpenseTracker.Common.Interfaces.UnitOfWork
{
    public interface IUnitOfWorkBase
    {
        IBaseResponse Execute(IBaseRequest request);
    }
}
