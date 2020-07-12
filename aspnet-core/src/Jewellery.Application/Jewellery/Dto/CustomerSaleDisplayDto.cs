using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerSaleDisplayDto : EntityDto<Guid>
    {
        public DateTime SaleDate { get; set; }

        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int SaleNumber { get; set; }

        public decimal? PaidAmount { get; set; }

        public ICollection<CustomerSaleDetailDisplayDto> SaleDetails { get; set; }

        public decimal TotalPrice => SaleDetails.Sum(s => s.SubTotal);

        public decimal? Due => TotalPrice - (PaidAmount.HasValue ? PaidAmount.Value : 0);

        public string SaleStatus { get; set; }
        public string PaymentStatus { get; set; }

    }
}
