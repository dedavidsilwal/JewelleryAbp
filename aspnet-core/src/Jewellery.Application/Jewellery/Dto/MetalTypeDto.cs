using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{

    public class MetalTypeDto : EntityDto<Guid>
    {

      [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }

    }

    public class CreateOrderDto : EntityDto<Guid>
    {
        public CreateOrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }

        [Required]
        public DateTime? RequiredDate { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaid { get; set; }

    }



}
