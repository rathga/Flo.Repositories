using System.Threading.Tasks;

namespace Flo.Repositories
{
    public interface IValidator<T, TKeyType> where T : Entity<TKeyType>
    {
        Task<Result> ValidateAsync(T entity);
    }
}