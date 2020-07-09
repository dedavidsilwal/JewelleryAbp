using System;

namespace Jewellery.Jewellery.Dto
{
    public class UpdatePaymentDto
    {
        public UpdatePaymentDto()
        {
            Status = PaymentStatus.Paid;
        }
        public Guid OrderId { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus Status { get; set; }
    }
}
