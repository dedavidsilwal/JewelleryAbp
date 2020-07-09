using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerOrderDto : EntityDto<Guid>
    {
        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string PhoneNumber { get; set; }
        public int OrderNumber { get; set; }

        public decimal AdvancePayment { get; set; }

        public ICollection<CustomerOrderDetailDto> OrderDetails { get; set; }

        public decimal TotalPrice => OrderDetails.Sum(s => s.SubTotal);

    }
}
