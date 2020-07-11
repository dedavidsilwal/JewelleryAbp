using System;

namespace Jewellery.Jewellery.Dto
{
    public class OrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public OrderDto Order { get; set; }
        public ProductDto Product { get; set; }


        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }


        public decimal TotalWeight { get; set; }
        public decimal SubTotal { get; set; }
    }
}
