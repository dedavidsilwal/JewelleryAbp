using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditSaleDto : EntityDto<Guid>
    {
        public CreateEditSaleDto()
        {
            SaleDetails = new HashSet<CreateEditSaleDetailDto>();
        }

        public int SaleNumber { get; set; }


        public Guid CustomerId { get; set; }

        public SaleStatus SaleStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public ICollection<CreateEditSaleDetailDto> SaleDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }

    }



}
