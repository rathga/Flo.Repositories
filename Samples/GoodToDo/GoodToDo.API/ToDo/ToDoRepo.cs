using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flo.Repositories;

namespace GoodToDo.API.ToDo
{
    public class ToDoRepo : RepositoryBase<ToDoItem, Guid>
    {
        public ToDoRepo(ICrud<ToDoItem, Guid> crud, IValidator<ToDoItem, Guid> validator, IAuthoriser<ToDoItem, Guid> authoriser) : base(crud, validator, authoriser)
        {

        }
    }
}
