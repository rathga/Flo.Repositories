using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Flo.Repositories
{
    public abstract class RepositoryBase<T, TKeyType> : IRepository<T, TKeyType> where T: Entity<TKeyType>
    {
        protected readonly IValidator<T, TKeyType> validator;
        protected readonly IAuthoriser<T, TKeyType> authoriser;
        protected readonly IRepository<T, TKeyType> crud;

        public RepositoryBase(IRepository<T, TKeyType> crud, IValidator<T, TKeyType> validator, IAuthoriser<T, TKeyType> authoriser)
        {
            this.crud = crud;
            this.validator = validator;
            this.authoriser = authoriser;
        }

        public virtual async Task<Result> AddAsync(T entity)
        {
            var result = await validator.ValidateAsync(entity);
            await authoriser.EnsureCanAddAsync(entity);

            if (result.Failure) return result;

            return await crud.AddAsync(entity);
        }

        public virtual async Task DeleteAsync(TKeyType id)
        {
            await authoriser.EnsureCanDeleteAsync(id);

            await crud.DeleteAsync(id);
        }

        public async Task<IRepoQuery<T, TKeyType>> GetAsync()
        {
            await authoriser.EnsureCanReadAsync();
            return await crud.GetAsync();
        }

        public async Task<T> GetByIdAsync(TKeyType id)
        {
            await authoriser.EnsureCanReadAsync(id);
            return await crud.GetByIdAsync(id);
        }

        public virtual async Task<Result> UpdateAsync(T entity)
        {
            var result = await validator.ValidateAsync(entity);
            await authoriser.EnsureCanUpdateAsync(entity);

            if (result.Failure) return result;

            await crud.UpdateAsync(entity);
            return Result.Ok();
        }

    }
}
