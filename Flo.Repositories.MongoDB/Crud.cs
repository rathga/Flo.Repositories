using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using Flo.Repositories;

namespace Flo.Repositories.MongoDB
{
    public class Crud<T, TKeyType> : ICrud<T, TKeyType> where T: Entity<TKeyType> 
    {
        private readonly IMongoCollection<T> collection;

        public Crud(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public virtual async Task<Result> AddAsync(T entity)
        {
            await collection.InsertOneAsync(entity);
            return Result.Ok();
        }

        public virtual async Task DeleteAsync(TKeyType id)
        {
            await collection.DeleteOneAsync(b => b.Id.Equals(id));
        }

        public async Task<IRepoQuery<T, TKeyType>> GetAsync()
        {
            return new RepoQuery<T, TKeyType>(collection.AsQueryable());
        }

        public async Task<T> GetByIdAsync(TKeyType id)
        {
            return await collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public virtual async Task<Result> UpdateAsync(T entity)
        {
            await collection.ReplaceOneAsync(b => b.Id.Equals(entity.Id), entity);
            return Result.Ok();
        }

    }
}
