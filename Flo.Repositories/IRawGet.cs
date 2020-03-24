using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace Flo.Repositories
{
    public interface IRawGet<T, TKeyType> where T: Entity<TKeyType>
    {
        IRepoQuery<T, TKeyType> Get();
    }
}
