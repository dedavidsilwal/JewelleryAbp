using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery.Dto
{
    public class OrderDto : EntityDto<Guid>
    {
        public OrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }
        public int OrderNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public Guid CustomerId { get; set; }
        public CustomerDto Customer { get; set; }


        public string Status { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }
    }
}
