using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;
using MongoDB.Driver;
using Flo.Repositories;

namespace Flo.Repositories.MongoDB
{
    public class RepoQuery<T, TKeyType> : RepoQueryBase<T, TKeyType> where T: Entity<TKeyType> 
    {
        private readonly IMongoQueryable<T> queryable;

        public RepoQuery(IMongoQueryable<T> queryable) : base(queryable)
        {
            this.queryable = queryable;
        }

        public override async Task<bool> AnyAsync()
        {
            return await queryable.AnyAsync();
        }

        public override async Task<T> FirstOrDefaultAsync()
        {
            return await queryable.FirstOrDefaultAsync();
        }

        public override async Task<List<T>> ToListAsync()
        {
            return await queryable.ToListAsync();
        }

        protected override IRepoQuery<TNext, TKeyType> Wrap<TNext>(IQueryable<TNext> queryable)
        {
            return new RepoQuery<TNext, TKeyType>((IMongoQueryable<TNext>)queryable);
        }
    }
}
