using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Requests
{
    public class CreateToDoListItemRequest
    {
        [Required]
        public string Name { get; set; }
    }
}