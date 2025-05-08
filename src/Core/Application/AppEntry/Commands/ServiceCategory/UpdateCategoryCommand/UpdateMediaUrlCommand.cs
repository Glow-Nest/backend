using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;

public class UpdateMediaUrlCommand : CategoryUpdateCommandBase
{
    internal List<MediaUrl> MediaUrls { get; }

    public UpdateMediaUrlCommand(CategoryId id, List<MediaUrl> mediaUrls) : base(id)
    {
        MediaUrls = mediaUrls;
    }
    
    public static Result<UpdateMediaUrlCommand> Create(string id, List<string> mediaUrls)
    {
        var idResult = CategoryId.FromGuid(Guid.Parse(id));
        var listOfErrors = new List<Error>();
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
            return Result<UpdateMediaUrlCommand>.Fail(listOfErrors);
        }
        
        return Result<UpdateMediaUrlCommand>.Success(new UpdateMediaUrlCommand(idResult, mediaUrlsList));
    }
}