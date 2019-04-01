using System.Threading.Tasks;

namespace ToDoList.Domain.Commands
{
    public interface IAddToDoListCommand : ICommand
    {
        string Name { get; }
    }
}
