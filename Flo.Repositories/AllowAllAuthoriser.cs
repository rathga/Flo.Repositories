using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public class AllowAllAuthoriser<T, TKeyType> : AuthoriserBase<T, TKeyType> where T: Entity<TKeyType>
    {
        public AllowAllAuthoriser(IRawGet<T, TKeyType> rawGet) : base(rawGet, new DummyAuthenticationService())
        {
        }

        public async override Task EnsureCanAddAsync(T entity)
        {
            // do nothing
        }

        public async override Task EnsureCanDeleteAsync(TKeyType id)
        {
            // do nothing
        }

        public async override Task EnsureCanReadAsync()
        {
            // do nothing
        }

        public async override Task EnsureCanReadAsync(TKeyType id)
        {
            // do nothing
        }

        public async override Task EnsureCanUpdateAsync(T entity)
        {
            // do nothing
        }

        class DummyAuthenticationService : IAuthenticationService
        {
            public string GetCurrentUserId()
            {
                return null;
            }
        }
    }
}
