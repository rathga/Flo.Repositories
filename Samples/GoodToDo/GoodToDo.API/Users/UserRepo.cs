using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flo.Repositories;

namespace GoodToDo.API.Users
{
    public class UserRepo : RepositoryBase<User, Guid>
    {
        public UserRepo(ICrud<User, Guid> crud, IValidator<User, Guid> validator, IAuthoriser<User, Guid> authoriser) : base(crud, validator, authoriser)
        {
        }
    }
}
