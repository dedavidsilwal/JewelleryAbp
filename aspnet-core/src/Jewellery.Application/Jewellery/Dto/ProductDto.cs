using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class ProductDto : EntityDto<Guid>
    {
        public string ProductName { get; set; }

        public Guid MetalTypeId { get; set; }
        public string MetalType { get; set; }

        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string Photo { get; set; }

        public short? UnitsInStock { get; set; }

    }

    public class ProductSearchResultDto : EntityDto<Guid>
    {
        public string ProductName { get; set; }
        public Guid MetalTypeId { get; set; }
        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }
        public string Photo { get; set; }

    }
}
