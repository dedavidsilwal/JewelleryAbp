using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerDto : EntityDto<Guid>
    {

        [Required]

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
