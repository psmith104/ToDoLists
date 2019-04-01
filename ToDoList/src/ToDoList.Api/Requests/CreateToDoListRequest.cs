using System.ComponentModel.DataAnnotations;
using ToDoList.Domain.Commands;

namespace ToDoList.Api.Requests
{
    public class CreateToDoListRequest : IAddToDoListCommand
    {
        [Required]
        public string Name { get; set; }
    }
}