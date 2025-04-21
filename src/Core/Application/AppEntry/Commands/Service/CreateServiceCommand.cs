using Domain.Aggregates.Service.Values;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Service;

public class CreateServiceCommand(Name name, Description description, Price price, TimeSpan duration, List<MediaUrl> mediaUrls)
{
    internal Name name = name;
    internal Description description = description;
    internal Price price = price;
    internal TimeSpan duration = duration;
    internal List<MediaUrl> mediaUrls = mediaUrls;
    
    
    public static Result<CreateServiceCommand> Create(string name, string description, double price, string duration, List<string> mediaUrls)
    {
        var listOfErrors = new List<Error>();
        
        var nameResult = Name.Create(name);
        if (!nameResult.IsSuccess)
        {
            listOfErrors.AddRange(nameResult.Errors);
        }

        var descriptionResult = Description.Create(description);
        if (!descriptionResult.IsSuccess)
        {
            listOfErrors.AddRange(descriptionResult.Errors);
        }

        var priceResult = Price.Create(price);
        if (!priceResult.IsSuccess)
        {
            listOfErrors.AddRange(priceResult.Errors);
        }
 
        var durationResult = TimeSpan.TryParse(duration, out var durationParsed);
        if (!durationResult)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingTime());
        }
        
        var mediaUrlsList = new List<MediaUrl>();
        foreach (var mediaUrl in mediaUrls)
        {
            var mediaUrlResult = MediaUrl.Create(mediaUrl);
            if (!mediaUrlResult.IsSuccess)
            {
                listOfErrors.AddRange(mediaUrlResult.Errors);
            }
            else
            {
                mediaUrlsList.Add(mediaUrlResult.Data);
            }
        }

        if (listOfErrors.Any())
        {
            return Result<CreateServiceCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateServiceCommand(nameResult.Data, descriptionResult.Data, priceResult.Data, durationParsed, mediaUrlsList);
        
        return Result<CreateServiceCommand>.Success(command);
    }
}
