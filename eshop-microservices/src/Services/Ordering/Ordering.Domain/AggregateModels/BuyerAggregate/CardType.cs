using MicroServiceDemo.Domain;

namespace Ordering.Domain.AggregateModels.BuyerAggregate;

public sealed class CardType : Entity<int>
{
    public string Name { get; init; }
}