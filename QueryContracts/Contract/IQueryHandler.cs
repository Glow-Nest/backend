namespace backend.Core.QueryContracts.Contract;

public interface IQueryHandler<in TQuery,TAnswer> where TQuery : IQuery<TAnswer>
{
    Task<TAnswer> HandleAsync(TQuery query);
}
