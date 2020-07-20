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
using System.Threading.Tasks;

namespace Jewellery.Jewellery
{
    [AbpAuthorize(PermissionNames.Pages_Customers)]

    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, Guid, PagedUserResultRequestDto, CustomerDto, CustomerDto>
    {
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
        }

        public async Task<CustomerDto[]> FetchAllCustomers() =>
            await Repository.GetAll().Select(x => ObjectMapper.Map<CustomerDto>(x)).ToArrayAsync();


        protected override IQueryable<Customer> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            var query = Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Address.Contains(input.Keyword) || x.DisplayName.Contains(input.Keyword) || x.PhoneNumber.Contains(input.Keyword));
            return query;
        }


    }


}
