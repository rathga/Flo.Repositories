using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Flo.Repositories
{
    public class DataAnnotationValidator<T, TKeyType> : IValidator<T, TKeyType> where T : Entity<TKeyType>
    {
        public virtual async Task<Result> ValidateAsync(T entity)
        {
            Result result = Result.Ok();

            // perform basic model validation based on data annotations
            var context = new ValidationContext(entity);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, context, results);

            if (!isValid)
            {
                result.Combine(Result.Fail(results.Select(vr => new Error { Key = vr.MemberNames.FirstOrDefault(), Message = vr.ErrorMessage }).ToArray()));
            }

            return result;
        }
    }
}