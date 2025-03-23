namespace Domain.Common.OperationResult;

public class Error(string errorId, string message)
{
    public string ErrorId { get; } = errorId;
    public string Message { get; } = message;
    
    public override bool Equals(object? obj)
    {
        if (obj is not Error other) return false;
        return ErrorId == other.ErrorId && Message == other.Message;
    }

    public override int GetHashCode() => HashCode.Combine(ErrorId, Message);
}