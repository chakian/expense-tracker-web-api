using System.Threading.Tasks;

namespace ExpenseTracker.Common.Interfaces.Command
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand model);
    }
}
