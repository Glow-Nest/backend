using Domain.Aggregates.Schedule.Entities;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;

namespace EfcQueries.Queries;

public class GetBlockedTimeQueryHandler : IQueryHandler<GetBlockedTime.Query, Result<GetBlockedTime.Answer>>
{
    private readonly DomainModelContext _context;

    public GetBlockedTimeQueryHandler(DomainModelContext context)
    {
        _context = context; 
    }

    public async Task<Result<GetBlockedTime.Answer>> HandleAsync(GetBlockedTime.Query query)
    {
        var scheduleDate = DateOnly.Parse(query.ScheduleDate);

        var blockedTimes = await _context.Set<BlockedTime>()
            .Where(time => time.ScheduledDate == scheduleDate)
            .ToListAsync();

        var blockedTimeDtos = blockedTimes
            .Select(bt => new GetBlockedTime.BlockedTimeDto(
                bt.TimeSlot.Start.ToString("HH:mm"),
                bt.TimeSlot.End.ToString("HH:mm"),
                bt.ScheduledDate.ToString("yyyy-MM-dd"),
                bt.Reason.Value))
            .ToList();

        return Result<GetBlockedTime.Answer>.Success(
            new GetBlockedTime.Answer(blockedTimeDtos));
    }
}