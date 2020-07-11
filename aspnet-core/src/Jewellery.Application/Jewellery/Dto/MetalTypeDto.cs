using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery.Dto
{

    public class MetalTypeDto : EntityDto<Guid>
    {

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

        public DateTime? RequiredDate { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaid { get; set; }

    }



}
