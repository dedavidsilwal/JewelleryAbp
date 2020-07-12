using Abp.Domain.Entities.Auditing;
using System;

namespace Jewellery.Jewellery
{
    public class Invoice : FullAuditedAggregateRoot<Guid>
    {

        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }


        public decimal PaidAmount { get; set; }

        public Guid? OrderId { get; set; }
        public Order Order { get; set; }

        public Guid? SaleId { get; set; }
        public Sale Sale { get; set; }

    }
}
