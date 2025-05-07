using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class UpdateCategoryCommand(CategoryId id, CategoryName name, CategoryDescription description, List<MediaUrl> mediaUrls)
{
    internal CategoryId id = id;
    internal CategoryName name = name;
    internal CategoryDescription description = description;
    internal List<MediaUrl> mediaUrls = mediaUrls;

    public static Result<UpdateCategoryCommand> Create(string id, string name, string description,
        List<string>? mediaUrls)
    {
        var listOfErrors = new List<Error>();
        var idResult = CategoryId.FromGuid(Guid.Parse(id));
        
        var nameResult = CategoryName.Create(name);
        if (!nameResult.IsSuccess)
        {
            listOfErrors.AddRange(nameResult.Errors);
        }

        var descriptionResult = CategoryDescription.Create(description);
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
            return Result<UpdateCategoryCommand>.Fail(listOfErrors);
        }

        var command = new UpdateCategoryCommand(idResult, nameResult.Data, descriptionResult.Data, mediaUrlsList);
        return Result<UpdateCategoryCommand>.Success(command);
    }
}