using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class AddServiceInCategoryCommand(ServiceName name, Price price, TimeSpan duration,CategoryId categoryId)
{
    internal readonly ServiceName name = name;
    internal readonly Price price = price;
    internal TimeSpan duration = duration;
    internal readonly CategoryId categoryId = categoryId;
    
    public static Result<AddServiceInCategoryCommand> Create(string namestr, double pricestr, string durationstr,string categoryIdstr)
    {
        var listOfErrors = new List<Error>();
        var serviceNameResult = ServiceName.Create(namestr);
        if (!serviceNameResult.IsSuccess)
        {
            listOfErrors.AddRange(serviceNameResult.Errors);
        }
        
        var priceResult = Price.Create(pricestr);
        if (!priceResult.IsSuccess)
        {
            listOfErrors.AddRange(priceResult.Errors);
        }
 
        var durationResult = TimeSpan.TryParse(durationstr, out var durationParsed);
        if (!durationResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingTime());
        }
        
        // client id
        var categoryIdParseResult = Guid.TryParse(categoryIdstr, out var categoryGuid);
        if (!categoryIdParseResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        var categoryIdResult = CategoryId.FromGuid(categoryGuid);
        
        if (listOfErrors.Any())
        {
            return Result<AddServiceInCategoryCommand>.Fail(listOfErrors);
        }
        
        var command = new AddServiceInCategoryCommand(
            serviceNameResult.Data,
            priceResult.Data,
            durationParsed,
            categoryIdResult
        );
        
        return Result<AddServiceInCategoryCommand>.Success(command);
    }
}