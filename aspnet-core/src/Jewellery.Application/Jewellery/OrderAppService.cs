using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Jewellery.Authorization;
using Jewellery.EntityFrameworkCore;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IRepository<Customer, Guid> _customerRepository;
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public OrderAppService(
            IRepository<Order, Guid> repository,
            IRepository<Invoice, Guid> invoiceRepository,
            IRepository<Customer, Guid> customerRepository,
            IRepository<Product, Guid> productRepository,
            IConfiguration configuration,
            IUnitOfWorkManager unitOfWorkManager
            ) : base(repository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _configuration = configuration;
            _unitOfWorkManager = unitOfWorkManager;
        }


        protected override IQueryable<Order> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = Repository.GetAllIncluding(x => x.Customer)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Customer.DisplayName.Contains(input.Keyword));
            return query;
        }

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            using var uow = _unitOfWorkManager.Begin();

            var order = ObjectMapper.Map<Order>(input);

            var orderResult = await _repository.InsertAsync(order);

            if (order.AdvancePaid.HasValue)
            {
                order.PaymentStatus = PaymentStatus.PartialPaid;

                var invoice = new Invoice
                {
                    InvoiceDate = DateTime.Now,
                    OrderId = order.Id,
                    Order = order,
                    PaidAmount = order.AdvancePaid.Value
                };

                await _invoiceRepository.InsertAsync(invoice);
            }

            await uow.CompleteAsync();

            return ObjectMapper.Map<OrderDto>(orderResult);
        }

        public Task UploadFile([FromForm] IFormFile file)
        {
            // Checking if files are sent to this app service.
            if (file.Length == 0)
                throw new UserFriendlyException("No files given.");

            return Task.CompletedTask;

        }

        public async Task<OrderDashboardDto[]> NearOrderDeliver()
        {
            var query = _repository
                .GetAll()
                .Where(s => s.OrderStatus == OrderStatus.Active)
                .Include(s=>s.OrderDetails)
                .OrderBy(s => s.RequiredDate)
                .Include(c => c.Customer)
                .Select(x => new OrderDashboardDto
                {
                    OrderNumber =x.OrderNumber,
                    AdvancePaid = x.AdvancePaid,
                    CustomerName = x.Customer.DisplayName,
                    OrderDate = x.OrderDate,
                    Id = x.Id,
                    RequiredDate = x.RequiredDate,
                    TotalAmount = x.Total,
                    TotalWeight = x.TotalWeight

                })
                .Take(5);

            return await query.ToArrayAsync();
        }

        public async Task<int> ActiveOrderCount() =>
            await _repository
            .GetAll()
            .Where(s => s.OrderStatus == OrderStatus.Active)
            .CountAsync();

        public async Task<int> TodayOrderPendingDeliveryCount() =>
                      await _repository
                      .GetAll()
                      .Where(s => s.OrderStatus == OrderStatus.Active && s.RequiredDate.HasValue && s.RequiredDate.Value.Date == DateTime.Today.Date)
                      .CountAsync();

        public async Task<decimal?> TodayAdvanceTaken() =>
              await _repository
              .GetAll()
              .Where(s => s.OrderStatus == OrderStatus.Active && s.OrderDate.Date == DateTime.Today.Date && s.AdvancePaid.HasValue)
              .SumAsync(s => s.AdvancePaid);

        public async Task<int> TodayTookOrderCount() =>
                      await _repository
                      .GetAll()
                      .Where(s => s.OrderStatus == OrderStatus.Active && s.OrderDate.Date == DateTime.Today.Date)
                      .CountAsync();

        public override async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            //orderby date desc

            var query = await _repository.GetAll()
                .Include(o => o.OrderDetails)
                .Include(c => c.Customer)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .OrderByDescending(s=>s.OrderDate)
                .Select(s => ObjectMapper.Map<OrderDto>(s))

                .ToListAsync();

            return new PagedResultDto<OrderDto>() { Items = query, TotalCount = await _repository.CountAsync() };
        }

        public async Task<EditOrderDto> FetchOrderWithDetails(Guid orderId)
        {
            return await _repository.GetAll()
                        .Include(p => p.OrderDetails)
                        .Where(x => x.Id == orderId)
                        .Select(x => new EditOrderDto
                        {
                            AdvancePaid = x.AdvancePaid,
                            CustomerName = x.Customer.DisplayName,
                            OrderNumber = x.OrderNumber,
                            Id = x.Id,
                            RequiredDate = x.RequiredDate,
                            OrderDetails = x.OrderDetails.Select(y => new CreateEditOrderDetailDto
                            {
                                MakingCharge = y.MakingCharge,
                                Wastage = y.Wastage,
                                Weight = y.Weight,
                                TodayMetalCost = y.TodayMetalCost,
                                MetalType = y.MetalType,
                                ProductId = y.ProductId,
                                Quantity = y.Quantity
                            }).ToList()

                        }).FirstOrDefaultAsync();
        }

        public async Task CancelAsync(Guid id)
        {

            using var uow = _unitOfWorkManager.Begin();

            var order = _repository.Get(id);
            order.OrderStatus = OrderStatus.Canceled;
            order.PaymentStatus = PaymentStatus.None;


            //if invoiced  delete

            var invoice = await _invoiceRepository.GetAll().Where(s => s.OrderId == id).FirstOrDefaultAsync();
            await _invoiceRepository.DeleteAsync(invoice);
            await _repository.UpdateAsync(order);
            await uow.CompleteAsync();
        }



        public override async Task<OrderDto> UpdateAsync(EditOrderDto input)
        {
            var builder = new DbContextOptionsBuilder<JewelleryDbContext>();
            var conn = _configuration.GetConnectionString("Default");
            builder.UseNpgsql(conn);

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


            return new PaymentOrderDto
            {
                AdvancePaid = order?.AdvancePaid,
                //OrderNumber = order.OrderNumber,
                CustomerName = (await _customerRepository.GetAll().FirstOrDefaultAsync(s => s.Id == order.CustomerId))?.DisplayName,
                OrderId = order.Id,
                TotalAmount = order.OrderDetails.Sum(s => s.SubTotal)
            };
        }


        public async Task<bool> UpdateOrderPaymentAsync(UpdatePaymentDto paymentDto)
        {


            using var uow = _unitOfWorkManager.Begin();

            var invoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                OrderId = paymentDto.OrderId,
                PaidAmount = paymentDto.PaidAmount
            };

            var order = _repository.Get(paymentDto.OrderId);

            await _repository.EnsureCollectionLoadedAsync(order, o => o.OrderDetails);

            var dueAmount = order.OrderDetails.Sum(s => s.SubTotal) - (order.AdvancePaid.HasValue ? order.AdvancePaid.Value : 0);

            if (dueAmount < paymentDto.PaidAmount)
            {
                throw new Exception("amount exceed");
            }
            else if (paymentDto.PaidAmount == dueAmount)
            {
                order.PaymentStatus = PaymentStatus.Paid;
            }
            else
            {
                order.PaymentStatus = PaymentStatus.PartialPaid;
            }

            order.OrderStatus = OrderStatus.Delivered;

            await _repository.UpdateAsync(order);

            await _invoiceRepository.InsertAsync(invoice);

            await uow.CompleteAsync();
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(OrderStatusChangeDto statusChangeDto)
        {

            var order = await _repository.GetAll()
                          .Include(p => p.OrderDetails)
                          .Where(x => x.Id == statusChangeDto.OrderId)
                          .FirstOrDefaultAsync();

            //update order status
            order.OrderStatus = statusChangeDto.OrderStatus;

            return true;
        }

        public async Task<CustomerOrderDisplayDto> FetchOrderDetail(Guid orderId)
        {
            var query = _repository.Get(orderId);
            await _repository.EnsureCollectionLoadedAsync(query, o => o.OrderDetails);

            var result = new CustomerOrderDisplayDto
            {

                CustomerName = _customerRepository.Get(query.CustomerId)?.DisplayName,
                CustomerAddress = _customerRepository.Get(query.CustomerId)?.Address,
                PhoneNumber = _customerRepository.Get(query.CustomerId)?.PhoneNumber,
                OrderDate = query.OrderDate,
                OrderNumber = query.OrderNumber,
                RequiredDate = query.RequiredDate,
                AdvancePaid = query.AdvancePaid,
                OrderStatus = query.OrderStatus.ToString(),
                PaymentStatus = query.PaymentStatus.ToString(),
                OrderDetails = query.OrderDetails.Select(o => new CustomerOrderDetailDisplayDto
                {
                    Quantity = o.Quantity,
                    MakingCharge = o.MakingCharge,
                    Weight = o.Weight,
                    Wastage = o.Wastage,
                    MetalType = o.MetalType,
                    TodayMetalCost = o.TodayMetalCost,
                    SubTotal = o.SubTotal,
                    TotalWeight = o.TotalWeight,
                    ProductName = _productRepository.Get(o.ProductId)?.ProductName
                }).ToList()

            };
            return result;

        }

    }


}
