using Domain.Aggregates.Service.Values;
using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Service;

public class Service : AggregateRoot
{
    internal ServiceId ServiceId { get;}


    public Service()
    {
        
    }
}