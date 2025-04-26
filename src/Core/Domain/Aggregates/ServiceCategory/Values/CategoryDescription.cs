using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory.Values;

public class CategoryDescription : ValueObject
{
    internal string Value { get; private set; }
    
    private CategoryDescription(string value)
    {
        Value = value;
    }
    
    public static Result<CategoryDescription> Create(string description)
    {
        
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result<CategoryDescription>.Fail(ServiceCategoryErrorMessage.EmptyServiceDescription());
        }
        
        return Result<CategoryDescription>.Success(new CategoryDescription(description));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}