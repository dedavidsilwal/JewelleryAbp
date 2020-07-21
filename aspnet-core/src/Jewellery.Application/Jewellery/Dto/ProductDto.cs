﻿using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;
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

    }
}
