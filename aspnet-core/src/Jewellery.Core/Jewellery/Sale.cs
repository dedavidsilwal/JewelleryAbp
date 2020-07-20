using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery
{
    public class Sale : FullAuditedAggregateRoot<Guid>
    {

        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
            Invoices = new HashSet<Invoice>();
            SaleStatus = SaleStatus.Sold;
            PaymentStatus = PaymentStatus.None;
            SalesDate = DateTime.Now;
        }


        public int SaleNumber { get; set; }

        public DateTime SalesDate { get; set; }


        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public SaleStatus SaleStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<Invoice> Invoices { get; set; }


        public decimal? PaidAmount { get; set; }

        public decimal TotalAmount => SaleDetails.Sum(s => s.SubTotal);
        public decimal TotalWeight => SaleDetails.Sum(s => s.TotalWeight);

        public decimal TotalPaidAmount => Invoices.Sum(s => s.PaidAmount);


    }
}
