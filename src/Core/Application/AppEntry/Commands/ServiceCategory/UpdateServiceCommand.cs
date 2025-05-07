using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class UpdateServiceCommand(CategoryId id, ServiceId serviceId, ServiceName name, Price price, TimeSpan duration)
{
    internal CategoryId id = id;
    internal ServiceId serviceId = serviceId;
    internal ServiceName name = name;
    internal Price price = price;
    internal TimeSpan duration = duration;

    public static Result<UpdateServiceCommand> Create(string id, string serviceId, string name, double price, TimeSpan duration)
    {
        var listOfErrors = new List<Error>();
        var idResult = CategoryId.FromGuid(Guid.Parse(id));
        
        var serviceIdResult = ServiceId.FromGuid(Guid.Parse(serviceId));
        
        var nameResult = ServiceName.Create(name);
        if (!nameResult.IsSuccess)
        {
            listOfErrors.AddRange(nameResult.Errors);
        }

        var priceResult = Price.Create(price);
        if (!priceResult.IsSuccess)
        {
            listOfErrors.AddRange(priceResult.Errors);
        }

        if (listOfErrors.Any())
        {
            return Result<UpdateServiceCommand>.Fail(listOfErrors);
        }

        var command = new UpdateServiceCommand(idResult, serviceIdResult, nameResult.Data, priceResult.Data, duration);
        return Result<UpdateServiceCommand>.Success(command);
    }
}