using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery.Dto
{
    public class OrderDueDetailDto
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<InvoiceDto> Invoices { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalPaid => Invoices.Sum(s => s.PaidAmount);
        public decimal? DueAmount => TotalAmount - TotalPaid;


    }

    public class SaleDueDetailDto
    {
        public Guid SaleId { get; set; }
        public string CustomerName { get; set; }
        public ICollection<InvoiceDto> Invoices { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TotalPaid => Invoices.Sum(s => s.PaidAmount);
        public decimal? DueAmount => TotalAmount - TotalPaid;


    }
}
