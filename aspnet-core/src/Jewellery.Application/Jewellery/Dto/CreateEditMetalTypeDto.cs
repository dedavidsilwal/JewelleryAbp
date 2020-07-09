using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditMetalTypeDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }
    }
}
