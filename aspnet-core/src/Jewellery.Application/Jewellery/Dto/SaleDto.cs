using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery.Dto
{
    public class SaleDto : EntityDto<Guid>
    {
        public SaleDto()
        {
            SaleDetails = new HashSet<SaleDetailDto>();
        }

        public int SaleNumber { get; set; }


        public DateTime SalesDate { get; set; }


        public Guid CustomerId { get; set; }

        public CustomerDto Customer { get; set; }

        public SaleStatus SaleStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<SaleDetailDto> SaleDetails { get; set; }

        public ICollection<InvoiceDto> Invoices { get; set; }

        
        public decimal? DueAmount { get; set; }

    }
}
