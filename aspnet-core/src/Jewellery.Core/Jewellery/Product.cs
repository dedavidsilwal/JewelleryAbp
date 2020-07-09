using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery
{
    public class Product : FullAuditedAggregateRoot<Guid>
    {

        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string ProductName { get; set; }

        public short? UnitsInStock { get; set; }
        public string Photo { get; set; }

        public Guid MetalTypeId { get; set; }
        public MetalType MetalType { get; set; }

        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; private set; }
    }
}
