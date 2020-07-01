using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Jewellery.Jewellery
{
    public class MetalType : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public WeightType WeightType { get; set; }

    }
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Invoices = new HashSet<Invoice>();
            Status = OrderStatus.Active;
            OrderDate = DateTime.Now;
        }

        public DateTime OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }

        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }

        public OrderStatus Status { get; set; }


        public ICollection<OrderDetail> OrderDetails { get; set; }

        public ICollection<Invoice> Invoices { get; set; }


        [NotMapped]
        public bool AdvancePaymentShow { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }


        [NotMapped]
        public decimal? TotalPayableAmount => OrderDetails.Sum(x => x.TotalPrice);
        [NotMapped]
        public decimal? TotalAmountShouldAmount => TotalPayableAmount - AdvancePaymentAmount;

    }

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
    public class Invoice : FullAuditedAggregateRoot<Guid>
    {
        public DateTime InvoiceDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }

        public decimal PaidAmount { get; set; }
        public int TotalPaymentAmount { get; set; }


        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        //[NotMapped]
        //public bool PartialPaid { get; set; }


        //[NotMapped]
        //public decimal RemaingAmount => PaymentStatus == PaymentStatus.PartialPayment ? TotalPaymentAmount - PaidAmount : default;
    }

    public class OrderDetail
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }
        //public float Discount { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public Order Order { get; set; }
        public Product Product { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }

        //[NotMapped]
        //public decimal NetPrice => (UnitPrice * Quantity) - (decimal)Discount;

        //[NotMapped]
        //public decimal DiscountPercentage => Quantity != 0 ? Math.Round((decimal)Discount / (UnitPrice * Quantity) * 100, 3) : 0;
    }
    public enum OrderStatus
    {
        Active,
        Canceled,
        Invoiced,
        PartialInvoiced
    }
    public enum PaymentStatus
    {
        Paid,
        PartialPayment,
        AdvancePayment
    }
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
    public class Sale : FullAuditedAggregateRoot<Guid>
    {
        public Sale()
        {
            SaleDetails = new HashSet<SaleDetail>();
        }


        public DateTime SalesDate { get; set; }


        public Guid CustomerId { get; set; }

        public Customer Customer { get; set; }
        public SaleStatus SaleStatus { get; set; }

        public PaymentStatus PaymentStatus { get; set; }


        public ICollection<SaleDetail> SaleDetails { get; set; }

        public ICollection<Invoice> Invoices { get; set; }


        [NotMapped]
        public bool AdvancePaymentShow { get; set; }

        public decimal? AdvancePaymentAmount { get; set; }



        [NotMapped]
        public decimal? TotalPayableAmount => SaleDetails.Sum(x => x.TotalPrice);
        [NotMapped]
        public decimal? TotalAmountShouldAmount => TotalPayableAmount - AdvancePaymentAmount;

    }
    public class SaleDetail
    {
        public Guid SaleId { get; set; }
        public Guid ProductId { get; set; }

        public short Quantity { get; set; }
        //public float Discount { get; set; }


        public decimal? Weight { get; set; }
        public decimal? MakingCharge { get; set; }
        public decimal? Wastage { get; set; }


        public Sale Sale { get; set; }
        public Product Product { get; set; }


        public string MetalType { get; set; }
        public decimal MetalCostThisDay { get; set; }


        public decimal UnitPrice { get; set; }

        public decimal TotalPrice { get; set; }
    }
    public enum SaleStatus
    {
        Sold,
        Canceled,
        Returned
    }
    public enum WeightType
    {
        Gram = 0,
        Tola = 1
    }
}
