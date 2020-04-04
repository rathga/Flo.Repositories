using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flo;
using Flo.Repositories;

namespace GoodToDo.API.Users
{
    public class MongoUserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        private readonly IRepository<User, Guid> userRepo;

        public MongoUserStore(IRepository<User, Guid> userRepo)
        {
            this.userRepo = userRepo;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            var r = await userRepo.AddAsync(user);

            return r.ToIdentityResult();
            
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await userRepo.DeleteAsync(user.Id);

            return IdentityResult.Success;
            
        }

        public void Dispose()
        {
            
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return await userRepo.GetByIdAsync(Guid.Parse(userId));
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return await userRepo.GetAsync().Where(u => u.NormalizedEmail == normalizedUserName).FirstOrDefaultAsync();
        }

        public async Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.NormalizedEmail;
        }

        public async Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return user.PasswordHash;
        }

        public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return user.Id.ToString();
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return user.Email;
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return !string.IsNullOrEmpty(user.PasswordHash);
        }

        public async Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedName;
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
        }

        public async Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.Email = userName;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            var r = await userRepo.UpdateAsync(user);

            return r.ToIdentityResult();
        }
    }

    internal static class ResultExtensions
    {
        public static IdentityResult ToIdentityResult(this Result r)
        {
            if (r.Success)
                return IdentityResult.Success;
            else 
                return IdentityResult.Failed(r.Errors.Select(e => new IdentityError { Code = e.Key, Description = e.Message }).ToArray());
        }
    }
}
