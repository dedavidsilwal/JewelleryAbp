using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
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
    [AbpAuthorize(PermissionNames.Pages_Products)]

    public class ProductAppService : AsyncCrudAppService<Product, ProductDto, Guid, PagedUserResultRequestDto, CreateEditProductDto, CreateEditProductDto>
    {
        public ProductAppService(IRepository<Product, Guid> repository) : base(repository)
        {
        }

       

        public async Task<ProductDto[]> FetchAll() => await Repository.GetAll().Include(x => x.MetalType).Select(x => ObjectMapper.Map<ProductDto>(x)).ToArrayAsync();

    }


}
