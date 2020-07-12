using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class SaleDto : EntityDto<Guid>
    {
        public int SaleNumber { get; set; }


        public DateTime SalesDate { get; set; }


        public string Customer { get; set; }

        public string SaleStatus { get; set; }

        public string PaymentStatus { get; set; }

        public decimal? PaidAmouunt { get; set; }

    }
}
