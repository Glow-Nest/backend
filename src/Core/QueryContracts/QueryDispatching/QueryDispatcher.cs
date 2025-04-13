using QueryContracts.Contracts;

namespace QueryContracts.QueryDispatching;

public class QueryDispatcher(IServiceProvider serviceProvider):IQueryDispatcher
{
    public Task<TAnswer> DispatchAsync<TAnswer>(IQuery<TAnswer> query)
    {
        Type queryInterfaceWithType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TAnswer));
        dynamic handler = serviceProvider.GetService(queryInterfaceWithType)!;
        return handler.HandleAsync((dynamic)query);
    }
}