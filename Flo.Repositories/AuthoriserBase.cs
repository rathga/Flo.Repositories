using System.Threading.Tasks;

namespace Flo.Repositories
{
    public abstract class AuthoriserBase<T, TKeyType> : IAuthoriser<T, TKeyType> where T: Entity<TKeyType>
    {
        protected readonly IRawGet<T, TKeyType> rawGet;
        protected readonly IAuthenticationService authenticationService;

        public AuthoriserBase(IRawGet<T, TKeyType> rawGet, IAuthenticationService authenticationService)
        {
            this.rawGet = rawGet;
            this.authenticationService = authenticationService;
        }

        public abstract Task EnsureCanAddAsync(T entity);


        public abstract Task EnsureCanDeleteAsync(TKeyType id);


        public abstract Task EnsureCanUpdateAsync(T entity);


        public abstract Task EnsureCanReadAsync();


        public abstract Task EnsureCanReadAsync(TKeyType id);


        protected string EnsureUserId()
        {
            string userId = authenticationService.GetCurrentUserId();
            if (userId == null) throw new NotAuthorisedException("User not logged in.");
            return userId;
        }
    }
}
