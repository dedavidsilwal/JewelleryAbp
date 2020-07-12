using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class OrderDashboardDto : EntityDto<Guid>
    {
        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }
        public string CustomerName { get; set; }

        public int OrderNumber { get; set; }

        public decimal? AdvancePaid { get; set; }

        public decimal? Total { get; set; }

    }
}
