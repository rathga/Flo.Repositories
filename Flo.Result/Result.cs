using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Flo
{
    public class Error
    {
        public string Key { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }

        internal static Error[] FromStringArray(string[] messages)
        {
            return messages?.Select(m => new Error { Key = "", Message = m }).ToArray();
        }
    }

    public class Result
    {
        public bool Success { get; private set; }
        public Error[] Errors { get; private set; }

        public bool Failure
        {
            get { return !Success; }
        }


        protected Result(bool success, string[] errors)
        {
            Contract.Requires(success || errors != null);
            Contract.Requires(!success || errors == null);

            Success = success;
            Errors = Error.FromStringArray(errors);
        }

        [JsonConstructor]
        public Result(bool success, Error[] errors)
        {
            Contract.Requires(success || errors != null);
            Contract.Requires(!success || errors == null);

            Success = success;
            Errors = errors;
        }

        public static Result Fail(string key, string message)
        {
            return new Result(false, new[] { new Error { Key = key, Message = message } });
        }

        public static Result Fail(string message)
        {
            return Fail("", message);
        }

        public static Result Fail(string[] errors)
        {
            return new Result(false, errors);
        }

        public static Result Fail(Error[] errors)
        {
            return new Result(false, errors);
        }

        public static Result<T> Fail<T>(string[] errors)
        {
            return new Result<T>(default(T), false, errors);
        }

        public static Result<T> Fail<T>(Error[] errors)
        {
            return new Result<T>(default(T), false, errors);
        }

        public static Result Ok()
        {
            return new Result(true, (Error[])null);
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, (Error[])null);
        }

        public Result Combine(params Result[] results)
        {
            List<Error> errors = null;
            if (Errors != null) errors = new List<Error>(Errors);

            foreach (Result result in results)
            {
                if (result.Failure)
                {
                    if (errors == null)
                    {
                        errors = new List<Error>(result.Errors);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            errors.Add(error);
                        }
                    }
                }
            }

            Success = errors == null;
            Errors = errors?.ToArray();

            return this;
        }
    }


    public class Result<T> : Result
    {
        private T _value;

        public T Value
        {
            get
            {
                Contract.Requires(Success);

                return _value;
            }
            private set { _value = value; }
        }

        protected internal Result(T value, bool success, string[] errors)
            : base(success, errors)
        {
            Contract.Requires(value != null || !success);

            Value = value;
        }

        protected internal Result(T value, bool success, Error[] errors)
    : base(success, errors)
        {
            Contract.Requires(value != null || !success);

            Value = value;
        }
    }
}