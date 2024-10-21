namespace Bookify.Domain.Abstractions;

public class Result
{
    protected internal Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != Error.None)
        {
            throw new InvalidOperationException();
        }

        if (!isSuccess && error == Error.None)
        {
            throw new InvalidOperationException();
        }
    }

    public bool IsSuccess { get; }
    public Error Error { get; }


    public bool IsFailure => !IsSuccess;
    public static Result Success() => new Result(true, Error.None);
    public static Result Failure(Error error) => new Result(false, error);


    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return value is not null
            ? Success(value)
            : Failure<TValue>(Error.NullValue);
    }
}

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    protected internal Result(TValue value, bool isSuccess, Error error) : base(isSuccess, error)
    {
        _value = value;
    }

    public TValue Value()
    {
        if (!IsSuccess)
        {
            throw new InvalidOperationException("The value of a failure result cannot be accessed.");
        }

        return _value;
    }

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}