using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.ClientPersistence;

public class ClientRepository : RepositoryBase<Client, ClientId>, IClientRepository
{
    private readonly DomainModelContext _context;

    public ClientRepository(DomainModelContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Result<Client>> GetAsync(Email email)
    {
        var client = await _context.Set<Client>()
            .FirstOrDefaultAsync(c => c.Email.Equals(email));

        return client is null
            ? Result<Client>.Fail(ClientErrorMessage.ClientNotFound())
            : Result<Client>.Success(client);
    }

    public async Task<Result<List<Client>>> GetAllAsync()
    {
        var clients = await _context.Set<Client>().ToListAsync();
        return Result<List<Client>>.Success(clients);
    }
}
