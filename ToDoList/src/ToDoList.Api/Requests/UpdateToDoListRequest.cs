using System.ComponentModel.DataAnnotations;
using ToDoList.Domain.Commands;

namespace ToDoList.Api.Requests
{
    public class UpdateToDoListRequest
    {
        [Required]
        public string Name { get; set; }
    }
}