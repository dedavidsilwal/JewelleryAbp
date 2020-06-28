﻿using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace Jewellery.Jewellery.Dto
{
    public class CreateEditMetalTypeDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }
    }

    public class MetalTypeDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }


        public string DisplayWeightType
        {
            get
            {
                return WeightType.ToString();
            }
        }
    }

    public class CreateOrderDto : EntityDto<Guid>
    {
        public CreateOrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }

        public DateTime? RequiredDate { get; set; }

        public Guid CustomerId { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }

    }

    public class PaymentOrderDto
    {
        public Guid OrderId { get; set; }
        public decimal? AdvancePayment { get; set; }
        public decimal? TotalAmount { get; set; }
    }

    public class UpdatePaymentDto
    {
        public Guid OrderId { get; set; }
        public decimal? PaidAmount { get; set; }


    }

    public class OrderStatusChangeDto
    {
        public Guid OrderId { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

    public class EditOrderDto : EntityDto<Guid>
    {
        public EditOrderDto()
        {
            OrderDetails = new HashSet<CreateEditOrderDetailDto>();
        }

        public DateTime? RequiredDate { get; set; }

        public Guid CustomerId { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<CreateEditOrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }

    }

    public class OrderDto : EntityDto<Guid>
    {
        public OrderDto()
        {
            OrderDetails = new HashSet<OrderDetailDto>();
        }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }


        public OrderStatus Status { get; set; }

        public ICollection<OrderDetailDto> OrderDetails { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }
    }

    public class CustomerDto : EntityDto<Guid>
    {

        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class InvoiceDto : EntityDto<Guid>
    {
        public string ReceiptNumber { get; set; }
        public string DocumentNumber { get; set; }

        public DateTime InvoiceDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public int PaidAmount { get; set; }
        public int TotalPaymentAmount { get; set; }


        public Guid OrderId { get; set; }
        public OrderDto Order { get; set; }


        public Guid SaleId { get; set; }
        public SaleDto Sale { get; set; }


        public BillType GetBillType()
        {
            return OrderId != default ? BillType.Order : BillType.Sale;
        }
    }

    public enum BillType
    {
        Order = 0,
        Sale = 1
    }
    public class CreateEditOrderDetailDto
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


    public class OrderDetailDto
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public OrderDto Order { get; set; }
        public ProductDto Product { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        //[NotMapped]
        //public decimal NetPrice => (UnitPrice * Quantity) - (decimal)Discount;

        //[NotMapped]
        //public decimal DiscountPercentage => Quantity != 0 ? Math.Round((decimal)Discount / (UnitPrice * Quantity) * 100, 3) : 0;
    }


    public class CreateEditProductDto : EntityDto<Guid>
    {

        public string ProductName { get; set; }

        public string Photo { get; set; }

        public Guid MetalTypeId { get; set; }
        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }
    }

    public class ProductDto : EntityDto<Guid>
    {

        public string ProductName { get; set; }

        public string Photo { get; set; }

        public Guid MetalTypeId { get; set; }
        public MetalTypeDto MetalType { get; set; }

        public decimal? EstimatedWeight { get; set; }
        public decimal? EstimatedCost { get; set; }
    }

    public class SaleDto : EntityDto<Guid>
    {
        public SaleDto()
        {
            SaleDetails = new HashSet<SaleDetailDto>();
        }


        public DateTime SalesDate { get; set; }


        public Guid CustomerId { get; set; }

        public CustomerDto Customer { get; set; }
        public SaleStatus SaleStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<SaleDetailDto> SaleDetails { get; set; }

        public ICollection<InvoiceDto> Invoices { get; set; }



        public decimal? AdvancePaymentAmount { get; set; }

    }
    public class SaleDetailDto
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }
        //public float Discount { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public SaleDto Sale { get; set; }
        public Product Product { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
