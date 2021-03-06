using Abp.Application.Services;
using Abp.Application.Services.Dto;
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
        private readonly IRepository<Product, Guid> _repository;
        private readonly IRepository<MetalType, Guid> _metalTypeRepository;

        public ProductAppService(
            IRepository<Product, Guid> repository,
            IRepository<MetalType, Guid> metalTypeRepository
            ) : base(repository)
        {
            _repository = repository;
            _metalTypeRepository = metalTypeRepository;
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

            return new PagedResultDto<ProductDto>() { Items = query, TotalCount = query.Count };
        }


        public async Task<ProductDto[]> FetchAll() =>
            await Repository
            .GetAll()
            .Include(x => x.MetalType)
            .Select(x => ObjectMapper.Map<ProductDto>(x))
            .ToArrayAsync();

    }


}
