using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class InvoiceDto : EntityDto<Guid>
    {
        public Guid OrderId { get; set; }
        public int InvoiceNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
        public string PaymentStatus { get; set; }

        public decimal PaidAmount { get; set; }
        public decimal TotalPaymentAmount { get; set; }

        public string CustomerName { get; set; }

    }
}
