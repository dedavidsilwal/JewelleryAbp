using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery
{
    public class Customer : FullAuditedAggregateRoot<Guid>
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Order> Orders { get; private set; }
    }
}
