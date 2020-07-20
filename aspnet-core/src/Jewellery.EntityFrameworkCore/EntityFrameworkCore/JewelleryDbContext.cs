using Abp.Zero.EntityFrameworkCore;
using Jewellery.Authorization.Roles;
using Jewellery.Authorization.Users;
using Jewellery.Jewellery;
using Jewellery.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.EntityFrameworkCore
{
    public class JewelleryDbContext : AbpZeroDbContext<Tenant, Role, User, JewelleryDbContext>
    {
        /* Define a DbSet for each entity of the application */


        public DbSet<Customer> Customers { get; set; }

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<SaleDetail> SaleDetails { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<MetalType> MetalTypes { get; set; }

        public JewelleryDbContext(DbContextOptions<JewelleryDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.HasSequence<int>("InvoiceNumbers")
                        .StartsAt(0001)
                        .IncrementsBy(1);


            modelBuilder.HasSequence<int>("OrderNumbers")
               .StartsAt(0001)
               .IncrementsBy(1);

            modelBuilder.HasSequence<int>("SaleNumbers")
                   .StartsAt(0001)
                   .IncrementsBy(1);

            modelBuilder.Entity<Invoice>()
                         .Property(o => o.InvoiceNumber)
                         .HasDefaultValueSql("nextval('\"InvoiceNumbers\"')");

            modelBuilder.Entity<Order>()
                         .Property(o => o.OrderNumber)
                         .HasDefaultValueSql("nextval('\"OrderNumbers\"')");

            modelBuilder.Entity<Sale>()
                  .Property(o => o.SaleNumber)
                  .HasDefaultValueSql("nextval('\"SaleNumbers\"')");

            modelBuilder.Entity<Customer>().Property(p => p.DisplayName)
                      .HasComputedColumnSql(@"""FirstName"" || ' ' || ""LastName""");


            modelBuilder.Entity<OrderDetail>().Property(p => p.TotalWeight)
                .HasComputedColumnSql("\"Wastage\"+\"Weight\"");

            modelBuilder.Entity<OrderDetail>().Property(p => p.SubTotal)
                .HasComputedColumnSql("(((\"Wastage\"+\"Weight\") * \"TodayMetalCost\") + \"MakingCharge\")*\"Quantity\"");


            modelBuilder.Entity<SaleDetail>().Property(p => p.TotalWeight)
            .HasComputedColumnSql("\"Wastage\"+\"Weight\"");

            modelBuilder.Entity<SaleDetail>().Property(p => p.SubTotal)
                .HasComputedColumnSql("(((\"Wastage\"+\"Weight\") * \"TodayMetalCost\") + \"MakingCharge\")*\"Quantity\"");


            //modelBuilder.Entity<Order>().Property(p => p.OrderDate).HasColumnType("DATETIME");
            //modelBuilder.Entity<Order>().Property(p => p.RequiredDate).HasColumnType("DATETIME");
            //modelBuilder.Entity<Order>().Property(p => p.ShippedDate).HasColumnType("DATETIME");

            //modelBuilder.Entity<Invoice>().Property(p => p.InvoiceDate).HasColumnType("DATETIME");


            //modelBuilder.Entity<Sale>().Property(p => p.SalesDate).HasColumnType("DATETIME");

            modelBuilder.Entity<OrderDetail>().HasKey(e => new { e.OrderId, e.ProductId });


            modelBuilder.Entity<SaleDetail>().HasKey(e => new { e.SaleId, e.ProductId });


            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.Order)
                   .WithMany(p => p.OrderDetails)
                   .HasForeignKey(d => d.OrderId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Order_Details_Orders");

            modelBuilder.Entity<SaleDetail>()
                .HasOne(d => d.Sale)
                   .WithMany(p => p.SaleDetails)
                   .HasForeignKey(d => d.SaleId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_Sale_Details_Sales");



            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Details_Products");

            modelBuilder.Entity<SaleDetail>()
                .HasOne(d => d.Product)
                .WithMany(p => p.SaleDetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Details_Products");

            modelBuilder.Entity<Product>()
             .Property(e => e.Photo);


            base.OnModelCreating(modelBuilder);
        }

    }
}
