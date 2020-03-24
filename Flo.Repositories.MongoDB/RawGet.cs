using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Flo.Repositories;

namespace Flo.Repositories.MongoDB
{
    public class RawGet<T, TKeyType> : IRawGet<T, TKeyType> where T: Entity<TKeyType>
    {
        protected readonly IMongoCollection<T> collection;

        public RawGet(IMongoCollection<T> collection)
        {
            this.collection = collection;
        }

        public virtual IRepoQuery<T, TKeyType> Get()
        {
            return new RepoQuery<T, TKeyType>(collection.AsQueryable());
        }
    }
}
