namespace Pdc.Autorizador.Dominio.Shared;

// Em: Pdc.Autorizador.Dominio/Compartilhado/Result.cs
using System.Collections.Generic;
using System.Linq;

namespace Pdc.Autorizador.Dominio.Compartilhado;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error[] Errors { get; }

    protected Result(bool isSuccess, Error[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Ok() => new(true, new[] { Error.None });
    public static Result<T> Ok<T>(T value) => new(value, true, new[] { Error.None });
    public static Result Falha(Error[] errors) => new(false, errors);
    public static Result<T> Falha<T>(Error[] errors) => new(default, false, errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(T? value, bool isSuccess, Error[] errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }
}