using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service.Values;

public class MediaUrl : ValueObject
{
    
    internal string Value { get; private set; }
    
    private MediaUrl(string value)
    {
        Value = value;
    }
    
    public static Result<MediaUrl> Create(string mediaUrl)
    {
        if (string.IsNullOrWhiteSpace(mediaUrl))
        {
            return Result<MediaUrl>.Fail(ServiceErrorMessage.EmptyServiceMediaUrl());
        }
        
        return Result<MediaUrl>.Success(new MediaUrl(mediaUrl));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}