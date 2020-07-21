using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditProductDto : EntityDto<Guid>
    {

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }


        public Guid MetalTypeId { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }

        public string Photo { get; set; }

    }
}
