using System.Threading.Tasks;
using ToDoList.Domain.Commands;

namespace ToDoList.Cache.CommandHandlers
{
    public class UpdateToDoListCommand : IAsyncCommandHandler<IUpdateToDoListCommand>
    {
        public async Task HandleAsync(IUpdateToDoListCommand command)
        {
            await Task.Delay(200).ConfigureAwait(false);
        }
    }
}
