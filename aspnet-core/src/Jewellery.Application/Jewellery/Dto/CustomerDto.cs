using Abp.Application.Services.Dto;
using System;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerDto : EntityDto<Guid>
    {

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
