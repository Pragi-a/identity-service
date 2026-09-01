using Identity.Application.Common.Errors;

namespace Identity.Application.Common.Results;

public class Result
{
    public bool IsSuccess { get; }

    public Error Error { get;}
    
    public bool IsFailure => !IsSuccess;


    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException(
                "A successful result cannot contain an error.");
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException(
                "A failed result must contain an error.");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new Result(true, Error.None);

    public static Result Failure(Error error) => new Result(false, error);
}

public sealed class Result<T> : Result
{

    private readonly T? _value;

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException(
            "Cannot access the value of a failed result.");

    private Result(bool isSuccess, Error error, T value)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static Result<T> Success(T value)
        => new(true, Error.None, value);

    public new static Result<T> Failure(Error error)
        => new(false, error, default!);
}