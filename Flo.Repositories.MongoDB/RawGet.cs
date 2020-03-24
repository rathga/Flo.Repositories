using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using RentalFlo.Repositories;

namespace RentalFlo.Repositories.MongoDB
{
    public class RawGet<T, TKeyType> : IRawGet<T, Guid> where T: Entity<Guid>
    {
        protected readonly IMongoCollection<T> collection;

        public RawGet(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public virtual IRepoQuery<T, Guid> Get()
        {
            return new RepoQuery<T>(collection.AsQueryable());
        }
    }
}
