using System;
using CompanyService.Infrastructure.Common;
using CompanyService.Infrastructure.Errors;
using Newtonsoft.Json;

#pragma warning disable SA1402 // File may only contain a single type
namespace CompanyService.Infrastructure.Models
{
    public class Result<TError>
        where TError : IError
    {
        public TError? Error { get; }

        public bool Succeeded => !Failed;

        public bool Failed => Error != null;

        protected Result()
        {
        }

        [JsonConstructor]
        protected Result(TError error)
        {
            Error = error;
        }

        public static Result<TError> Ok()
        {
            return new Result<TError>();
        }

        public static Result<TError, TResult> Ok<TResult>(TResult result)
        {
            return new Result<TError, TResult>(result);
        }

        public static Result<TError> Fail(TError error)
        {
            error.ThrowIfNull();
            return new Result<TError>(error);
        }

        public static Result<TError, TResult> Fail<TResult>(TError error)
        {
            error.ThrowIfNull();
            return new Result<TError, TResult>(error);
        }
    }

    public class Result<TError, TResult> : Result<TError>
        where TError : IError
    {
        private readonly TResult _result;

        public TResult Value => Failed ? throw new InvalidOperationException() : _result;

        protected internal Result(TResult value)
        {
            _result = value;
        }

        protected internal Result(TError error) : base(error)
        {
        }

        [JsonConstructor]
        protected Result(TResult value, TError error) : base(error)
        {
            _result = value;
        }
    }
}
