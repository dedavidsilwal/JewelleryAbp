using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerOrderDisplayDto : EntityDto<Guid>
    {
        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int OrderNumber { get; set; }

        public decimal? AdvancePaid { get; set; }

        public ICollection<CustomerOrderDetailDisplayDto> OrderDetails { get; set; }

        public decimal TotalPrice => OrderDetails.Sum(s => s.SubTotal);

        public decimal? Due => TotalPrice - (AdvancePaid.HasValue ? AdvancePaid.Value : 0);

        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }

    }
}
