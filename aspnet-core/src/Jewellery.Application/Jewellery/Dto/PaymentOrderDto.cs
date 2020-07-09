using System;

namespace Jewellery.Jewellery.Dto
{
    public class PaymentOrderDto
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public Guid OrderId { get; set; }
        public decimal? AdvancePayment { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
