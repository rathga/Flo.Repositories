using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public class Entity<TKeyType>
    {
        public TKeyType Id { get; set; }
    }
}
