using System.Diagnostics.CodeAnalysis;

namespace Pdc.Autorizador.Dominio.Shared;
[ExcludeFromCodeCoverage]
public class Result
{
    public virtual bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; }

    protected Result()
    {
        IsSuccess = true;
        Error = Error.None;
    }
    protected Result(Error error)
    {
        IsSuccess = false;
        Error = error;
    }

    public static Result Success() => new();
    public static Result Failure(Error error) => new(error);
}

[ExcludeFromCodeCoverage]
public class Result<T> : Result
{
    [MemberNotNullWhen(true, nameof(Value))]
    public override bool IsSuccess { get; }
    private Result(T value) : base()
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(Error error) : base(error)
    {
        IsSuccess = false;
        Value = default(T);
    }
    public T? Value { get; }

    public static Result<T> Success(T value) => new(value);
    public static new Result<T> Failure(Error error) => new(error);
}