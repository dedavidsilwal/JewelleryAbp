using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Jewellery.Authorization;
using Jewellery.Jewellery.Dto;
using Jewellery.Users.Dto;
using System;

namespace Jewellery.Jewellery
{
    [AbpAuthorize(PermissionNames.Pages_Orders)]

    public class SaleAppService : AsyncCrudAppService<Sale, SaleDto, Guid, PagedUserResultRequestDto, CreateEditSaleDto, CreateEditSaleDto>
    {
        public SaleAppService(IRepository<Sale, Guid> saleRepository) : base(saleRepository)
        {

        }
    }
}
