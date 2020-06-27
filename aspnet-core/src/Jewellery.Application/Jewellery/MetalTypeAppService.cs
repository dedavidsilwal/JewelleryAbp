using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Jewellery.Authorization;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using System;

namespace Jewellery.Jewellery
{

    public interface IJewelleryAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_MetalTypes)]

    public class MetalTypeAppService : AsyncCrudAppService<MetalType, MetalTypeDto, Guid, PagedUserResultRequestDto, MetalTypeDto, MetalTypeDto>, IJewelleryAppService
    {
        public MetalTypeAppService(IRepository<MetalType, Guid> repository) : base(repository)
        {
        }
    }

    public interface ICustomerAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_Customers)]

    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, Guid, PagedUserResultRequestDto, CustomerDto, CustomerDto>, IJewelleryAppService
    {
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
        }
    }


    public interface IProductAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_Products)]

    public class ProductAppService : AsyncCrudAppService<Product, ProductDto, Guid, PagedUserResultRequestDto, ProductDto, ProductDto>, IProductAppService
    {
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
        }
    }

    public interface IOrderAppService
    {

    }


    [AbpAuthorize(PermissionNames.Pages_Orders)]

    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, Guid, PagedUserResultRequestDto, CreateOrderDto, EditOrderDto>, IOrderAppService
    {
        public OrderAppService(IRepository<Order, Guid> repository) : base(repository)
        {
        }
    }


}
