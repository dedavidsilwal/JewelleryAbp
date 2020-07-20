using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
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
    [AbpAuthorize(PermissionNames.Pages_MetalTypes)]

    public class MetalTypeAppService : AsyncCrudAppService<MetalType, MetalTypeDto, Guid, PagedUserResultRequestDto, CreateEditMetalTypeDto, CreateEditMetalTypeDto>
    {
        public MetalTypeAppService(IRepository<MetalType, Guid> repository) : base(repository)
        {
        }

        protected override IQueryable<MetalType> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword));
            return query;
        }

        public async Task<MetalTypeDto[]> FetchAllMetalTypes() => await Repository
            .GetAll()
            .Select(x => ObjectMapper.Map<MetalTypeDto>(x))
            .ToArrayAsync();


        public async Task<decimal?> FetchTodayMetalPrice(string metalType) => (await Repository
            .FirstOrDefaultAsync(s => s.Name == metalType))?.Price;

    }
}
