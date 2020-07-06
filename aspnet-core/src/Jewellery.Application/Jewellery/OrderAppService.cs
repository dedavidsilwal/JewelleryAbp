using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Jewellery.Authorization;
using Jewellery.EntityFrameworkCore;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{
    [AbpAuthorize(PermissionNames.Pages_Orders)]

    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, Guid, PagedUserResultRequestDto, CreateOrderDto, EditOrderDto>
    {
        private readonly IRepository<Order, Guid> _repository;
        private readonly IRepository<Invoice, Guid> _invoiceRepository;
        private readonly IConfiguration _configuration;

        public OrderAppService(
            IRepository<Order, Guid> repository,
            IRepository<Invoice, Guid> invoiceRepository,
            IConfiguration configuration
            ) : base(repository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _configuration = configuration;
        }


        public async Task<OrderDto[]> NearOrderDeliver()
        {
            var query = _repository
                .GetAll()
                .OrderBy(s => s.RequiredDate)
                .Take(5);

            return await ObjectMapper.ProjectTo<OrderDto>(query).ToArrayAsync();
        }

        public async Task<int> TotalOrderCount() =>
            await _repository
            .GetAll()
            .Where(s => s.Status == OrderStatus.Active)
            .CountAsync();

        public override async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedUserResultRequestDto input)
        {

            var query = await _repository.GetAll()
                .Include(o => o.OrderDetails)
                .Include(c => c.Customer)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount).Select(s => ObjectMapper.Map<OrderDto>(s))
                .ToListAsync();

            return new PagedResultDto<OrderDto>() { Items = query, TotalCount = query.Count };
        }

        public async Task<EditOrderDto> FetchOrderWithDetails(Guid orderId) =>
          await _repository.GetAll()
            .Include(p => p.OrderDetails)
            .Where(x => x.Id == orderId)
            .Select(x => ObjectMapper.Map<EditOrderDto>(x)).FirstOrDefaultAsync();




        public override async Task<OrderDto> UpdateAsync(EditOrderDto input)
        {
            var builder = new DbContextOptionsBuilder<JewelleryDbContext>();
            var conn = _configuration.GetConnectionString("Default");
            builder.UseSqlServer(conn);

            using var context = new JewelleryDbContext(builder.Options);
            using var transaction = context.Database.BeginTransaction();
            var orderEntity = ObjectMapper.Map<Order>(input);

            var existingOrder = await context.Orders.Include(s => s.OrderDetails).FirstOrDefaultAsync(x => x.Id == input.Id);


            context.Entry(existingOrder).CurrentValues.SetValues(orderEntity);

            foreach (var detail in orderEntity.OrderDetails)
            {
                detail.OrderId = orderEntity.Id;

                var existingDetail = existingOrder.OrderDetails.FirstOrDefault(s => s.ProductId == detail.ProductId);
                if (existingDetail == null)
                {
                    existingOrder.OrderDetails.Add(detail);
                }
                else
                {
                    context.Entry(existingDetail).CurrentValues.SetValues(detail);
                }

            }

            foreach (var detail in existingOrder.OrderDetails)
            {
                if (!orderEntity.OrderDetails.Any(p => p.ProductId == detail.ProductId))
                {
                    context.Remove(detail);
                }
            }

            await context.SaveChangesAsync();

            // Commit transaction if all commands succeed, transaction will auto-rollback
            // when disposed if either commands fails
            transaction.Commit();

            var result = await context.Orders.Include(s => s.OrderDetails).FirstOrDefaultAsync(x => x.Id == input.Id);
            return ObjectMapper.Map<OrderDto>(result);
        }

        public async Task<PaymentOrderDto> GetPaymentOrderDtoAsync(Guid orderId)
        {

            var order = await _repository.GetAll()
                          .Include(p => p.OrderDetails)
                          .Where(x => x.Id == orderId)
                          .FirstOrDefaultAsync();


            //need calculation
            return new PaymentOrderDto
            {
                AdvancePayment = order?.AdvancePaymentAmount,
                OrderId = order.Id,
                TotalAmount = order.OrderDetails?.Sum(s => s.TotalPrice)
            };
        }


        public async Task<bool> UpdateOrderPaymentAsync(UpdatePaymentDto paymentDto)
        {

            //var order = await _repository.GetAll()
            //    .Include(x => x.Invoices).Where(x => x.Id == paymentDto.Id)
            //              .FirstOrDefaultAsync();



            //update order status
            //generate invoice

            var invoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                OrderId = paymentDto.OrderId,
                PaidAmount = paymentDto.PaidAmount,
                PaymentStatus = paymentDto.Status,
            };
            //order.Invoices.Add(invoice);
            //await _invoiceRepository.UpdateAsync(invoice);


            await _invoiceRepository.InsertAsync(invoice);

            return true;
        }

        // public async Task<bool> UpdateSalePaymentAsync(UpdatePaymentDto paymentDto)
        // {

        //     // var order = await _repository.GetAll()
        //     //               .Where(x => x.Id == paymentDto.OrderSaleId)
        //     //               .FirstOrDefaultAsync();

        //     var invoice = new Invoice
        //     {
        //         SaleId = paymentDto.Id,
        //         PaidAmount = paymentDto.PaidAmount,
        //         PaymentStatus = paymentDto.Status,
        //     };
        //     await _invoiceRepository.InsertAsync(invoice);

        //     return true;
        // }

        public async Task<bool> UpdateOrderStatusAsync(OrderStatusChangeDto statusChangeDto)
        {

            var order = await _repository.GetAll()
                          .Include(p => p.OrderDetails)
                          .Where(x => x.Id == statusChangeDto.OrderId)
                          .FirstOrDefaultAsync();

            //update order status
            order.Status = statusChangeDto.OrderStatus;

            return true;
        }

    }


}
