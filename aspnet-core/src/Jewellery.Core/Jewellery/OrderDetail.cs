using System;

namespace Jewellery.Jewellery
{
    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }

        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }

        public Order Order { get; set; }
        public Product Product { get; set; }


        public string MetalType { get; set; }
        public decimal TodayMetalCost { get; set; }

        public decimal TotalWeight { get; private set; }
        public decimal SubTotal { get; private set; }


        //public decimal TotalWeight => (Weight.HasValue ? Weight.Value : 0) + (Wastage.HasValue ? Wastage.Value : 0);

        //public decimal SubTotal => (TotalWeight * TodayMetalCost) + (MakingCharge.HasValue ? MakingCharge.Value : 0);
    }
}
