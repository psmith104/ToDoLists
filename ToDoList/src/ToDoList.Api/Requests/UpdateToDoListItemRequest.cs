using System.ComponentModel.DataAnnotations;

namespace ToDoList.Api.Requests
{
    public class UpdateToDoListItemRequest
    {
        [Required]
        public string Name { get; set; }
    }
}