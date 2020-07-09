using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditProductDto : EntityDto<Guid>
    {

        public string ProductName { get; set; }

        public string Photo { get; set; }

        public Guid MetalTypeId { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }
    }
}
