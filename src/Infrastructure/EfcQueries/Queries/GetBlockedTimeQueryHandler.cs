using Domain.Aggregates.Schedule.Entities;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;

namespace EfcQueries.Queries;

public class GetBlockedTimeQueryHandler : IQueryHandler<GetBlockedTimeQuery, Result<GetBlockedTimeResponse>>
{
    private readonly DomainModelContext _context;

    public GetBlockedTimeQueryHandler(DomainModelContext context)
    {
        _context = context; 
    }

    public async Task<Result<GetBlockedTimeResponse>> HandleAsync(GetBlockedTimeQuery query)
    {
        var scheduleDate = DateOnly.Parse(query.ScheduleDate);

        var blockedTimes = await _context.Set<BlockedTime>()
            .Where(time => time.ScheduledDate == scheduleDate)
            .ToListAsync();

        var blockedTimeDtos = blockedTimes
            .Select(bt => new BlockedTimeDto(
                bt.TimeSlot.Start.ToString("HH:mm"),
                bt.TimeSlot.End.ToString("HH:mm"),
                bt.ScheduledDate.ToString("yyyy-MM-dd"),
                bt.Reason.Value))
            .ToList();

        return Result<GetBlockedTimeResponse>.Success(
            new GetBlockedTimeResponse(blockedTimeDtos));
    }
}