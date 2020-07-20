using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
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
        private readonly IRepository<Product, Guid> _repository;
        private readonly IRepository<MetalType, Guid> _metalTypeRepository;
        private readonly ICacheManager cacheManager;

        public ProductAppService(
            IRepository<Product, Guid> repository,
            IRepository<MetalType, Guid> metalTypeRepository,
            ICacheManager cacheManager
            ) : base(repository)
        {
            _repository = repository;
            _metalTypeRepository = metalTypeRepository;
            this.cacheManager = cacheManager;
        }




        public override async Task<PagedResultDto<ProductDto>> GetAllAsync(PagedUserResultRequestDto input)
        {

            var query = await (from p in _repository.GetAll()
                               join m in _metalTypeRepository.GetAll() on p.MetalTypeId equals m.Id
                               select new ProductDto
                               {
                                   Id = p.Id,
                                   ProductName = p.ProductName,
                                   EstimatedWeight = p.EstimatedWeight,
                                   EstimatedCost = p.EstimatedCost,
                                   MetalType = m.Name,
                               })
                              .Skip(input.SkipCount)
                              .Take(input.MaxResultCount)
                              .ToListAsync();

            return new PagedResultDto<ProductDto>() { Items = query, TotalCount = await _repository.CountAsync() };
        }


        protected override IQueryable<Product> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = Repository.GetAllIncluding(x => x.MetalType)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.ProductName.Contains(input.Keyword) || x.MetalType.Name.Contains(input.Keyword));
            return query;
        }

        public async Task<ProductDto[]> FetchAll() =>
            await Repository
            .GetAll()
            .Include(x => x.MetalType)
            .Select(x => ObjectMapper.Map<ProductDto>(x))
            .ToArrayAsync();

    }


}
