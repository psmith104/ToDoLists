using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Domain.Models;

namespace ToDoList.Cache.Models
{
    public class ToDoListModel : IToDoList
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ToDoListModel()
        {

        }

        public ToDoListModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
