using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Jewellery.Dto
{
    public class EditOrderDto : EntityDto<Guid>
    {
        public EditOrderDto()
        {
            OrderDetails = new HashSet<CreateEditOrderDetailDto>();
        }
        public int OrderNumber { get; set; }


        public DateTime? RequiredDate { get; set; }

        public string CustomerName { get; set; }


        public ICollection<CreateEditOrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaid { get; set; }



    }



}
