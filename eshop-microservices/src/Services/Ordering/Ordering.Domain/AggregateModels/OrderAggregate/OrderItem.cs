using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroServiceDemo.Domain;
using Ordering.Domain.Exceptions;

namespace Ordering.Domain.AggregateModels.OrderAggregate
{
    /// <summary>
    /// 实体，非聚合根
    /// </summary>
    public class OrderItem : Entity<long>
    {
        [Required]
        public string ProductName { get; private set; }

        public string PictureUrl { get; private set; }

        public decimal UnitPrice { get; private set; }

        public decimal Discount { get; private set; }

        public int Units { get; private set; }

        public int ProductId { get; private set; }

        protected OrderItem()
        { }

        public OrderItem(int productId, string productName, decimal unitPrice, decimal discount, string pictureUrl, int units = 1)
        {
            if (units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            if (unitPrice * units < discount)
            {
                throw new OrderingDomainException("The total of order item is lower than applied discount");
            }

            ProductId = productId;

            ProductName = productName;
            UnitPrice = unitPrice;
            Discount = discount;
            Units = units;
            PictureUrl = pictureUrl;
        }

        public void SetNewDiscount(decimal discount)
        {
            if (discount < 0)
            {
                throw new OrderingDomainException("Discount is not valid");
            }

            Discount = discount;
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }

            Units += units;
        }
    }
}