using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class SalesReportDashboard : EntityDto<Guid>
    {
        public int saleNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime SaleDate { get; set; }
    }
}
