using System;

namespace Jewellery.Jewellery.Dto
{
    public class PaymentOrderDto
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public Guid OrderId { get; set; }
        public decimal? AdvancePaid { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal Due => TotalAmount - (AdvancePaid.HasValue ? AdvancePaid.Value : 0);
    }
}
