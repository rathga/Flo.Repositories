using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public abstract class RepoQueryBase<T, TKeyType> : IRepoQuery<T, TKeyType> where T : Entity<TKeyType>
    {

        protected readonly IQueryable<T> _dbSet;

        public RepoQueryBase(IQueryable<T> dbSet)
        {
            _dbSet = dbSet;
        }

        public IRepoQuery<T, TKeyType> Where(Expression<Func<T, bool>> predicate)
        {
            return Wrap(_dbSet.Where(predicate));
        }

        public IRepoQuery<TResult, TKeyType> Select<TResult>(Expression<Func<T, TResult>> selector) where TResult : Entity<TKeyType>
        {
            return Wrap(_dbSet.Select(selector));
        }

        protected abstract IRepoQuery<TNext, TKeyType> Wrap<TNext>(IQueryable<TNext> queryable) where TNext : Entity<TKeyType>;

        public abstract Task<T> FirstOrDefaultAsync();
        public abstract Task<bool> AnyAsync();
        public abstract Task<List<T>> ToListAsync();
    }

}
