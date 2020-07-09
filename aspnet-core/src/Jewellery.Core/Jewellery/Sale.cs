using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery
{
    public class Sale : FullAuditedAggregateRoot<Guid>
    {

        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
            Invoices = new HashSet<Invoice>();
            SaleStatus = SaleStatus.Sold;
            SalesDate = DateTime.Now;
        }


        public DateTime SalesDate { get; set; }


        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public SaleStatus SaleStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<Invoice> Invoices { get; set; }


        public decimal? DueAmount { get; set; }

    }
}
