using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditSaleDto : EntityDto<Guid>
    {
        public CreateEditSaleDto()
        {
            SaleDetails = new HashSet<CreateEditSaleDetailDto>();
        }

        [Required]
        public Guid CustomerId { get; set; }


        public ICollection<CreateEditSaleDetailDto> SaleDetails { get; set; }

        public decimal? PaidAmount { get; set; }

    }

}
