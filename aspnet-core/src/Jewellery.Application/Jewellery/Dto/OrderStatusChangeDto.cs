using System;

namespace Jewellery.Jewellery.Dto
{
    public class OrderStatusChangeDto
    {
        public Guid OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
