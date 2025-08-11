using MicroServiceDemo.Domain.Abstractions;

namespace Ordering.Domain.AggregateModels.BuyerAggregate;

public interface IBuyerRepository : IRepository<Buyer>
{
}