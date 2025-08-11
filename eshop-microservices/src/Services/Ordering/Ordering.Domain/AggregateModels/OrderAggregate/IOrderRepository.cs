using MicroServiceDemo.Domain.Abstractions;

namespace Ordering.Domain.AggregateModels.OrderAggregate;

public interface IOrderRepository : IRepository<Order>
{
}