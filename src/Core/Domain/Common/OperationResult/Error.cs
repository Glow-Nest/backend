namespace Domain.Common.OperationResult;

public class Error(string errorId, string message)
{
    public string ErrorId { get; } = errorId;
    public string Message { get; } = message;
}