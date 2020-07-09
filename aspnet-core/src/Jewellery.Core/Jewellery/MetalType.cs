using Abp.Domain.Entities.Auditing;
using System;

namespace Jewellery.Jewellery
{
    public class MetalType : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }

    }
}
