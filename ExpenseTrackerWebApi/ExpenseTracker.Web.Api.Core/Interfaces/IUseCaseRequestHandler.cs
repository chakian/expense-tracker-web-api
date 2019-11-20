using System.Threading.Tasks;

namespace ExpenseTracker.Web.Api.Core.Interfaces
{
    public interface IUseCaseRequestHandler<in TUseCaseRequest, out TUseCaseResponse> where TUseCaseRequest : IUseCaseRequest<TUseCaseResponse>
    {
        Task<bool> Handle(TUseCaseRequest message, IOutputPort<TUseCaseResponse> outputPort);
    }
}
