using Abp.Application.Services;
using Jewellery.MultiTenancy.Dto;

namespace Jewellery.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

