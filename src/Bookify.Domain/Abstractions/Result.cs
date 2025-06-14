using System;
using System.Diagnostics.CodeAnalysis;

namespace Bookify.Domain.Abstractions
{
    public class Result
    {
        protected internal Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException("Success result cannot have an error.");
            }
            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException("Failure result must have an error.");
            }
            IsSuccess = isSuccess;
            Error = error;
        }
        public bool IsSuccess { get; private set; }
        public Error Error { get; private set; }
        public bool IsFailure => !IsSuccess;
        public static Result Success()
        {
            return new Result(true, Error.None);
        }
        public static Result Failure(Error error)
        {
            return new Result(false, error);
        }
        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new(value,true, Error.None);
        }
        public static Result<TValue> Failure<TValue>(Error error)
        {
            return new(default, false, error);
        }
        public static Result<TValue> Create<TValue>(TValue? value)
        {
            if (value is null)
            {
                return Failure<TValue>(Error.NullValue);
            }
            return Success(value);
        }
    }
    public class Result<TValue> : Result
    {
        public readonly TValue _value;
        protected internal Result(TValue? value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }
        [NotNull]
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("Cannot access value of a failure result.");
            
        public static implicit operator Result<TValue>(TValue value)=> Create(value);
    }
}
