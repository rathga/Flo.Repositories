using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public interface IRepository<T, TKeyType> where T : Entity<TKeyType>
    {
        Task<Result> AddAsync(T entity);
        Task<Result> UpdateAsync(T entity);
        Task DeleteAsync(TKeyType id);
        Task<IRepoQuery<T, TKeyType>> GetAsync();
        Task<T> GetByIdAsync(TKeyType id);

    }
}
