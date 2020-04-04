using Flo.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodToDo.API.Users
{
    public class User : Entity<Guid>
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
    }
}
