using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MicroServiceDemo.Domain;
using Ordering.Domain.AggregateModels.BuyerAggregate;

namespace Ordering.Domain.AggregateModels.OrderAggregate
{
    public class Order : Entity<long>, IAggregateRoot
    {
        public DateTime OrderDate { get; private set; }

        // Address is a Value Object pattern example persisted as EF Core 2.0 owned entity
        [Required]
        public Address Address { get; private set; }

        public long? BuyerId { get; private set; }

        public Buyer Buyer { get; }

        public OrderStatus OrderStatus { get; private set; }

        public string Description { get; private set; }

        // Draft orders have this set to true. Currently we don't check anywhere the draft status of an Order, but we could do it if needed
#pragma warning disable CS0414 // The field 'Order._isDraft' is assigned but its value is never used
        private bool _isDraft;
#pragma warning restore CS0414

        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so OrderItems cannot be added from "outside the AggregateRoot" directly to the collection,
        // but only through the method OrderAggregateRoot.AddOrderItem() which includes behavior.
        private readonly List<OrderItem> _orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

        public int? PaymentId { get; private set; }

        public static Order NewDraft()
        {
            var order = new Order
            {
                _isDraft = true
            };
            return order;
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }
    }
}