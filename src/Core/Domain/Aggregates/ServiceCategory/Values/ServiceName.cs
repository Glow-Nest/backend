using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory.Values;

public class ServiceName : ValueObject
{
    
    internal string Value { get; private set; }
    
    private ServiceName(string value)
    {
        Value = value;
    }
    
    public static Result<ServiceName> Create(string name)
    {
        
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<ServiceName>.Fail(ServiceCategoryErrorMessage.EmptyServiceName());
        }
        
        return Result<ServiceName>.Success(new ServiceName(name));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}