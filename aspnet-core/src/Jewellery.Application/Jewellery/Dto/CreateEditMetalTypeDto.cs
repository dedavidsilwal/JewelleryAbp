using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditMetalTypeDto : EntityDto<Guid>
    {
        [Required]
        [StringLength(100)]

        public string Name { get; set; }
      
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }
    }
}
