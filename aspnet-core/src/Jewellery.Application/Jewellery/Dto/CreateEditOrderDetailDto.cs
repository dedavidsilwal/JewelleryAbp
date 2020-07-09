using System;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditOrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
