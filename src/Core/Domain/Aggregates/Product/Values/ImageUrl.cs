using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Product.Values;

public class ImageUrl : ValueObject
{
    internal string Value { get; private set; }
    
    private ImageUrl(string value)
    {
        Value = value;
    }
    
    public static Result<ImageUrl> Create(string imageUrl)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Result<ImageUrl>.Fail(ProductErrorMessage.EmptyImageUrl());
        }
        return Result<ImageUrl>.Success(new ImageUrl(imageUrl));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}