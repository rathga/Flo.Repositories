using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Flo;
using Flo.Repositories;
using GoodToDo.API.ToDo;

namespace GoodToDo.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ToDoController : ControllerBase
    {
        private readonly IRepository<ToDoItem, Guid> toDoRepo;

        public ToDoController(IRepository<ToDoItem, Guid> toDoRepo)
        {
            this.toDoRepo = toDoRepo;
        }

        [HttpPost]
        public async Task<Result<Guid>> Add(ToDoItem toDo)
        {
            var result = await toDoRepo.AddAsync(toDo);
            if (result.Success)
            {
                return Result.Ok(toDo.Id);
            }
            else
            {
                return Result.Fail<Guid>(result.Errors);
            }
        }

        [HttpPost]
        public async Task<Result> Update(ToDoItem toDo)
        {
            return await toDoRepo.UpdateAsync(toDo);
        }

        [HttpGet]
        public async Task<IEnumerable<ToDoItem>> GetForUser(Guid userId)
        {
            return await toDoRepo.GetAsync().Where(t => t.UserId == userId).ToListAsync(); // TODO: change to .GetByUser authorised method
        }

        [HttpPost]
        public async Task Delete(Guid id)
        {
            await toDoRepo.DeleteAsync(id); 
        }

    }
}
