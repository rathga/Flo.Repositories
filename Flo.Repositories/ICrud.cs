using System;
using System.Collections.Generic;
using System.Text;

namespace Flo.Repositories
{
    public interface ICrud<T, TKeyType> : IRepository<T, TKeyType> where T : Entity<TKeyType>
    {
    }
}
