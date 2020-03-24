using System;
using System.Threading.Tasks;

namespace Flo
{
    public static class ResultExtensions
    {
        public static Result OnSuccess(this Result result, Func<Result> func)
        {
            if (result.Failure)
                return result;

            return func();
        }

        public static async Task<Result> OnSuccessAsync(this Result result, Func<Task<Result>> func)
        {
            if (result.Failure)
                return result;

            return await func();
        }

        public static Result OnSuccess(this Result result, Action action)
        {
            if (result.Failure)
                return result;

            action();

            return Result.Ok();
        }

        public static async Task OnSuccessAsync(this Result result, Func<Task> func)
        {
            if (!result.Failure)
                await func();
        }

        public static Result OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.Failure)
                return result;

            action(result.Value);

            return Result.Ok();
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<T> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors);

            return Result.Ok(func());
        }

        public static Result<T> OnSuccess<T>(this Result result, Func<Result<T>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors);

            return func();
        }

        public async static Task<Result<T>> OnSuccessAsync<T>(this Result result, Func<Task<Result<T>>> func)
        {
            if (result.Failure)
                return Result.Fail<T>(result.Errors);

            return await func();
        }


        public static Result OnSuccess<T>(this Result<T> result, Func<T, Result> func)
        {
            if (result.Failure)
                return result;

            return func(result.Value);
        }

        public static Result OnFailure(this Result result, Action action)
        {
            if (result.Failure)
            {
                action();
            }

            return result;
        }

        public static Result OnBoth(this Result result, Action<Result> action)
        {
            action(result);

            return result;
        }

        public static T OnBoth<T>(this Result result, Func<Result, T> func)
        {
            return func(result);
        }
    }
}
