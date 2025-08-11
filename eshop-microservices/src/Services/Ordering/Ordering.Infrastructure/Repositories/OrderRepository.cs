using MicroServiceDemo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : Repository<Order, long, OrderingContext>, IOrderRepository
{
    public OrderRepository(OrderingContext dbContext) : base(dbContext)
    {
        try
        {
        }
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entityEntry in ex.Entries)
            {
                //entityEntry.Entity;
            }
            Console.WriteLine(ex);
            throw;
        }
    }
}