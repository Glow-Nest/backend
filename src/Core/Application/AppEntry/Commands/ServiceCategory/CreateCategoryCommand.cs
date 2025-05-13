using Domain.Aggregates.ServiceCategory.Values;
using OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class CreateCategoryCommand (CategoryName name, CategoryDescription description, List<MediaUrl> mediaUrls)
{
    internal CategoryName name = name;
    internal CategoryDescription description = description;
    internal List<MediaUrl> mediaUrls = mediaUrls;

    public static Result<CreateCategoryCommand> Create(string namestr, string descriptionstr, List<string>? mediaUrls)
    {
        var listOfErrors = new List<Error>();
        var nameResult = CategoryName.Create(namestr);
        if (!nameResult.IsSuccess)
        {
            listOfErrors.AddRange(nameResult.Errors);
        }
        
        var descriptionResult = CategoryDescription.Create(descriptionstr);
        if (!descriptionResult.IsSuccess)
        {
            listOfErrors.AddRange(descriptionResult.Errors);
        }
        
        var mediaUrlsList = new List<MediaUrl>();
        if (mediaUrls != null)
        {
            foreach (var mediaUrl in mediaUrls)
            {
                if (string.IsNullOrWhiteSpace(mediaUrl)) continue;
                
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
        }
        
        if (listOfErrors.Any())
        {
            return Result<CreateCategoryCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateCategoryCommand(nameResult.Data, descriptionResult.Data, mediaUrlsList);
        return Result<CreateCategoryCommand>.Success(command);
    }
}