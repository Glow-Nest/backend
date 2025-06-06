﻿using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using OperationResult;

namespace DomainModelPersistence.Repositories;

public class SalonOwnerRepository : RepositoryBase<SalonOwner, SalonOwnerId>, ISalonOwnerRepository
{
    private readonly DomainModelContext _context;

    public SalonOwnerRepository(DomainModelContext context) : base(context)
    {
        _context = context;
    }
    
    public async Task<Result<SalonOwner>> GetAsync(Email email)
    {
        var salonOwner = await _context.Set<SalonOwner>()
            .FirstOrDefaultAsync(c => c.Email.Equals(email));

        return salonOwner is null
            ? Result<SalonOwner>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound())
            : Result<SalonOwner>.Success(salonOwner);
    }
}