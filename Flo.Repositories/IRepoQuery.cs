using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Flo.Repositories;

namespace Flo.Repositories
{

    public interface IRepoQuery<T, TKeyType>
    {
        Task<T> FirstOrDefaultAsync();
        Task<bool> AnyAsync();
        Task<List<T>> ToListAsync();
        IRepoQuery<T, TKeyType> Where(Expression<Func<T, bool>> predicate);
        IRepoQuery<TResult, TKeyType> Select<TResult>(Expression<Func<T, TResult>> selector) where TResult : Entity<TKeyType>;
    }
}
namespace  System.Threading.Tasks
{
    public static class IRepoQueryTaskExtensions
    {
        public static Task<T> FirstOrDefaultAsync<T, TKeyType>(this Task<IRepoQuery<T, TKeyType>> task) where T : Entity<TKeyType>
        {
            return task.Result.FirstOrDefaultAsync();
        }
        public static Task<bool> AnyAsync<T, TKeyType>(this Task<IRepoQuery<T, TKeyType>> task) where T : Entity<TKeyType>
        {
            return task.Result.AnyAsync();
        }
        public static async Task<List<T>> ToListAsync<T, TKeyType>(this Task<IRepoQuery<T, TKeyType>> task) where T : Entity<TKeyType>
        {
            await task;
            return await task.Result.ToListAsync();
        }
        public static IRepoQuery<T, TKeyType> Where<T, TKeyType>(this Task<IRepoQuery<T, TKeyType>> task, Expression<Func<T, bool>> predicate) where T : Entity<TKeyType>
        {
            return task.Result.Where(predicate);
        }

        public static IRepoQuery<TResult, TKeyType> Select<T, TResult, TKeyType>(this Task<IRepoQuery<T, TKeyType>> task, Expression<Func<T, TResult>> selector) where T : Entity<TKeyType> where TResult : Entity<TKeyType>
        {
            return task.Result.Select(selector);
        }
    }
}
