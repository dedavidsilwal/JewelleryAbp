using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace Jewellery.Jewellery.Dto
{
    public class CustomerDto : EntityDto<Guid>
    {

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }


        public string Address { get; set; }
        public string PhoneNumber { get; set; }


        public string DisplayName { get; private set; }

    }


    public class CustomerSearchResultDto : EntityDto<Guid>
    {
        public string DisplayName { get;  set; }

        public string Address { get; set; }
        public string PhoneNumber { get; set; }

    }
}
