namespace OperationResult;

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
    
    public bool HasError(string code)
    {
        return Errors.Any(e => e.ErrorId == code);
    }
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
    
    public bool HasError(string code)
    {
        return Errors.Any(e => e.ErrorId == code);
    }
}

public static class ResultExtensions
{
    public static Result<T> ToGeneric<T>(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful Result to Result<T>.");

        return Result<T>.Fail(result.Errors);
    }
    
    
    public static Result ToNonGeneric<T>(this Result<T> result)
    {
        if (result.IsSuccess)
            return Result.Success();

        return Result.Fail(result.Errors);
    }
    
    public static Result<None> ToNone(this Result result)
    {
        if (result.IsSuccess)
            return Result<None>.Success(None.Value);

        return Result<None>.Fail(result.Errors);
    }

}

