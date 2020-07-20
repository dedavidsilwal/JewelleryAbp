using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditSaleDetailDto
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        [Required]
        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }

        [Required]

        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }


        public decimal TotalWeight => (Weight.HasValue ? Weight.Value : 0) + (Wastage.HasValue ? Wastage.Value : 0);

        public decimal SubTotal => (TotalWeight * TodayMetalCost) + (MakingCharge.HasValue ? MakingCharge.Value : 0);

    }



}
