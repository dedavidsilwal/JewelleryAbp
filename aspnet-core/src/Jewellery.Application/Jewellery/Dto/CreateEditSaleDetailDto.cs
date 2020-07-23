﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditSaleDetailDto
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }

        [Required]
        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }

        [Required]

        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }


        public decimal TotalWeight { get; set; }

        public decimal TotalPrice { get; set; }

    }
}
