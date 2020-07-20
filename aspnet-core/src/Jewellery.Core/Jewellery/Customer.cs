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

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }


        public ICollection<Order> Orders { get; private set; }

        public string DisplayName { get; private set; }

    }
}
