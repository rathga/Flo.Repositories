using System;
using System.Threading.Tasks;
using RentalFlo.Shared;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver.Linq;
using RentalFlo.Repositories;

namespace RentalFlo.Repositories.MongoDB
{
    public class Crud<T> : ICrud<T, Guid> where T: Entity<Guid>
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

        public virtual async Task DeleteAsync(Guid id)
        {
            await collection.DeleteOneAsync(b => b.Id == id);
        }

        public async Task<IRepoQuery<T, Guid>> GetAsync()
        {
            return new RepoQuery<T>(collection.AsQueryable());
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public virtual async Task<Result> UpdateAsync(T entity)
        {
            await collection.ReplaceOneAsync(b => b.Id == entity.Id, entity);
            return Result.Ok();
        }

    }
}
