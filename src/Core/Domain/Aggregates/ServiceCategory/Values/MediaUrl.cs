using OperationResult;

namespace Domain.Aggregates.ServiceCategory.Values;

public class MediaUrl
{
    
    public string Value { get; private set; }
    
    private MediaUrl() { } // For EFC
    
    public MediaUrl(string value)
    {
        Value = value;
    }
    
    public static Result<MediaUrl> Create(string mediaUrl)
    {
        if (string.IsNullOrWhiteSpace(mediaUrl))
        {
            return Result<MediaUrl>.Fail(ServiceCategoryErrorMessage.EmptyServiceMediaUrl());
        }
        
        return Result<MediaUrl>.Success(new MediaUrl(mediaUrl));
    }
}