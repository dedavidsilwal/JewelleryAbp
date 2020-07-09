using System;

namespace Jewellery.Jewellery
{
    public class SaleDetail
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }



        public Sale Sale { get; set; }
        public Product Product { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }

        public decimal SubTotal { get; set; }
    }
}
