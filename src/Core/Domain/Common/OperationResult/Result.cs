namespace Domain.Common.OperationResult;

public class Result<T>
{
    public T Data { get; }
    public bool IsSuccess => Errors.Count == 0;
    public List<Error> Errors { get; } = new();

    private Result(T data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data), "Data cannot be null in a successful result.");
    }

    private Result(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("At least one error must be provided.", nameof(errors));

        Errors = errors;
        Data = default!;
    }

    public static Result<T> Success(T data) => new(data);

    public static Result<T> Fail(List<Error> errors) => new(errors);

    public static Result<T> Fail(Error error) =>
        new([error]);
}

public class Result
{
    public bool IsSuccess => Errors.Count == 0;
    public List<Error> Errors { get; } = new();

    private Result()
    {
    }

    private Result(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("At least one error must be provided.", nameof(errors));

        Errors = errors;
    }

    public static Result Success() => new();

    public static Result Fail(List<Error> errors) => new(errors);

    public static Result Fail(Error error) =>
        new([error]);
    
}

public static class ResultExtensions
{
    public static Result<T> ToGeneric<T>(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful Result to Result<T>.");

        return Result<T>.Fail(result.Errors);
    }
}