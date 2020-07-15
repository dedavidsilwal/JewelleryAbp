using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditOrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        [Required]
        public short Quantity { get; set; }

        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }

        [Required]
        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }

    }
}
