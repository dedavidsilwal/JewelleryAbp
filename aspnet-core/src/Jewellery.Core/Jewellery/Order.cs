using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Invoices = new HashSet<Invoice>();
            Status = OrderStatus.Active;
            OrderDate = DateTime.Now;
            PaymentStatus = PaymentStatus.PartialPayment;
        }

        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<Invoice> Invoices { get; set; }


        public decimal? AdvancePaymentAmount { get; set; }

    }
}
