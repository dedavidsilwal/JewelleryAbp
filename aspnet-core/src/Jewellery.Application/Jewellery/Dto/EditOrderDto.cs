using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

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

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<CreateEditOrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }

    }

    public class EditSaleDto : EntityDto<Guid>
    {
        public EditSaleDto()
        {
            SaleDetails = new HashSet<CreateEditSaleDetailDto>();
        }

        public int SaleNumber { get; set; }


        public Guid CustomerId { get; set; }

        public SaleStatus SaleStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public ICollection<CreateEditSaleDetailDto> SaleDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }

    }


    public class CreateEditSaleDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }



}
