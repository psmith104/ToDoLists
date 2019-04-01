using System.Threading.Tasks;

namespace ToDoList.Domain.Commands
{
    public interface IAddToDoListCommand : ICommand
    {
        string Name { get; }
    }

    public class AddToDoListCommandHandler : IAsyncCommandHandler<IAddToDoListCommand>
    {
        public Task HandleAsync(IAddToDoListCommand command)
        {
            return Task.FromResult(0);
        }
    }
}
