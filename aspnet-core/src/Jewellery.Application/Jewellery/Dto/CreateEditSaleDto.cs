using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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


        public decimal? TotalAmount => SaleDetails.Sum(s => s.SubTotal);

    }

    public class EditSaleDto : EntityDto<Guid>
    {
        public EditSaleDto()
        {
            SaleDetails = new HashSet<CreateEditSaleDetailDto>();
        }

        [Required]
        public string CustomerName { get; set; }

        public int SaleNumber { get; set; }


        public ICollection<CreateEditSaleDetailDto> SaleDetails { get; set; }

        public decimal? PaidAmount { get; set; }


        public decimal? TotalAmount => SaleDetails.Sum(s => s.SubTotal);

    }

}
