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

    public static Result<T> Failure(List<Error> errors) => new(errors);

    public static Result<T> Failure(string errorId, string message) =>
        new([new Error(errorId, message)]);
}

public class Result
{
    public bool IsSuccess => Errors.Count == 0;
    public List<Error> Errors { get; } = new();

    private Result() { }

    private Result(List<Error> errors)
    {
        if (errors == null || errors.Count == 0)
            throw new ArgumentException("At least one error must be provided.", nameof(errors));

        Errors = errors;
    }

    public static Result Success() => new();

    public static Result Failure(List<Error> errors) => new(errors);

    public static Result Failure(string errorId, string message) =>
        new([new Error(errorId, message)]);
}