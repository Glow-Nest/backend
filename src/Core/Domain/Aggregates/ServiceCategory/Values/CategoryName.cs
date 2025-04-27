using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory.Values;

public class CategoryName : ValueObject
{
    internal string Value { get; private set; }

    private CategoryName(string value)
    {
        Value = value;
    }
    
    public static Result<CategoryName> Create(string name)
    {
        
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<CategoryName>.Fail(ServiceCategoryErrorMessage.EmptyServiceName());
        }
        
        return Result<CategoryName>.Success(new CategoryName(name));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        throw new NotImplementedException();
    }
}