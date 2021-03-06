﻿using Abp.Application.Services.Dto;
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


        public string PaymentStatus { get; set; }

        public string OrderStatus { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaid { get; set; }

        public decimal? Total { get; set; }
    }
}
