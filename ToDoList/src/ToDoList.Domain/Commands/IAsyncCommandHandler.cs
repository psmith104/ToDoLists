using System.Threading.Tasks;

namespace ToDoList.Domain.Commands
{
    public interface IAsyncCommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
