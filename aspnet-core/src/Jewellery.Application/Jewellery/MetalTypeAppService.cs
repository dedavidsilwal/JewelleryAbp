using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.ObjectMapping;
using Jewellery.Authorization;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{

    public interface IJewelleryAppService
    {

    }

    public class InvoiceAppService : ApplicationService
    {
        private readonly IRepository<Invoice, Guid> _repository;
        private readonly IObjectMapper _objectMapper;

        public InvoiceAppService(IRepository<Invoice, Guid> repository, IObjectMapper objectMapper)
        {
            _repository = repository;
            _objectMapper = objectMapper;
        }


        public async Task<PagedResultDto<InvoiceDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            var query = await _repository
                .GetAll().Include(x => x.Order)
                .Take(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(x => _objectMapper.Map<InvoiceDto>(x))
                .ToListAsync();

            var result = new PagedResultDto<InvoiceDto>
            {
                Items = query,
                TotalCount = query.Count
            };

            return result;
        }
    }


    [AbpAuthorize(PermissionNames.Pages_MetalTypes)]

    public class MetalTypeAppService : AsyncCrudAppService<MetalType, MetalTypeDto, Guid, PagedUserResultRequestDto, CreateEditMetalTypeDto, CreateEditMetalTypeDto>, IJewelleryAppService
    {
        public MetalTypeAppService(IRepository<MetalType, Guid> repository) : base(repository)
        {
        }

        public async Task<MetalTypeDto[]> FetchAllMetalTypes() => await Repository.GetAll().Select(x => ObjectMapper.Map<MetalTypeDto>(x)).ToArrayAsync();


        public async Task<decimal?> FetchTodayMetalPrice(string metalType) => (await Repository
            .FirstOrDefaultAsync(s => s.Name == metalType))?.Price;

    }



    [AbpAuthorize(PermissionNames.Pages_Customers)]

    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, Guid, PagedUserResultRequestDto, CustomerDto, CustomerDto>
    {
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
        }

        public async Task<CustomerDto[]> FetchAllCustomers() => await Repository.GetAll().Select(x => ObjectMapper.Map<CustomerDto>(x)).ToArrayAsync();

        public async Task<CustomerDto[]> SearchQueryCustomers(string search) =>
            await Repository.GetAll()
            .Where(x => x.CustomerName.Contains(search))
            .Select(x => ObjectMapper.Map<CustomerDto>(x))
            .ToArrayAsync();
    }



    public interface IProductAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_Products)]

    public class ProductAppService : AsyncCrudAppService<Product, ProductDto, Guid, PagedUserResultRequestDto, CreateEditProductDto, CreateEditProductDto>, IProductAppService
    {
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
        }

        public async Task<ProductDto[]> FetchAll() => await Repository.GetAll().Include(x => x.MetalType).Select(x => ObjectMapper.Map<ProductDto>(x)).ToArrayAsync();

    }

    public interface IOrderAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_Orders)]

    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, Guid, PagedUserResultRequestDto, CreateOrderDto, EditOrderDto>, IOrderAppService
    {
        private readonly IRepository<Order, Guid> _repository;

        public OrderAppService(IRepository<Order, Guid> repository) : base(repository)
        {
            _repository = repository;
        }

        public override Task<PagedResultDto<OrderDto>> GetAllAsync(PagedUserResultRequestDto input)
        {
            return base.GetAllAsync(input);
        }

        public async Task<EditOrderDto> FetchOrderWithDetails(Guid orderId) =>
          await _repository.GetAll()
            .Include(p => p.OrderDetails)
            .Where(x => x.Id == orderId)
            .Select(x => ObjectMapper.Map<EditOrderDto>(x)).FirstOrDefaultAsync();



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


        public async Task<bool> UpdatePaymentAsync(UpdatePaymentDto paymentDto)
        {

            var order = await _repository.GetAll()
                          .Include(p => p.OrderDetails)
                          .Where(x => x.Id == paymentDto.OrderId)
                          .FirstOrDefaultAsync();

            //update order status
            //generate invoice

            return default;
        }

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
