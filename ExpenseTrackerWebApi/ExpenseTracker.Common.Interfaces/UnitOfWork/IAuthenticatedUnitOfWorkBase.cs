using ExpenseTracker.Common.Interfaces.Models;

namespace ExpenseTracker.Common.Interfaces.UnitOfWork
{
    public interface IAuthenticatedUnitOfWorkBase
    {
        IBaseAuthenticatedResponse Execute(IBaseAuthenticatedRequest request);
    }
}
