using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flo.Repositories;

namespace GoodToDo.API.ToDo
{
    public class ToDoItem : Entity<Guid>
    {
        public Guid UserId { get; set; }

        [Required]
        public string Message { get; set; }

        public bool Complete { get; set; }
    }
}
