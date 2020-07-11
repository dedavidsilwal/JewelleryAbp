namespace Jewellery.Jewellery.Dto
{
    public class CustomerOrderDetailDisplayDto
    {
        public short Quantity { get; set; }

        public decimal? MakingCharge { get; set; }

        public decimal? Weight { get; set; }

        public decimal? Wastage { get; set; }


        public string MetalType { get; set; }
        public string ProductName { get; set; }
        public decimal TodayMetalCost { get; set; }

        public decimal TotalWeight { get; set; }
        public decimal SubTotal { get; set; }

    }
}
