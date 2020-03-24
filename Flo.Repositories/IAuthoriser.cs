using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public interface IAuthoriser<T, TKeyType> where T: Entity<TKeyType>
    {
        Task EnsureCanAddAsync(T entity);
        Task EnsureCanDeleteAsync(TKeyType id);
        Task EnsureCanReadAsync();
        Task EnsureCanReadAsync(TKeyType id);
        Task EnsureCanUpdateAsync(T entity);
    }
}
