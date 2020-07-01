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
    [AbpAuthorize(PermissionNames.Pages_Customers)]

    public class CustomerAppService : AsyncCrudAppService<Customer, CustomerDto, Guid, PagedUserResultRequestDto, CustomerDto, CustomerDto>
    {
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
        }

        public async Task<CustomerDto[]> FetchAllCustomers() => 
        await Repository.GetAll().Select(x => ObjectMapper.Map<CustomerDto>(x)).ToArrayAsync();

        public async Task<CustomerDto[]> SearchQueryCustomers(string search) =>
            await Repository.GetAll()
            .Where(x => x.CustomerName.Contains(search))
            .Select(x => ObjectMapper.Map<CustomerDto>(x))
            .ToArrayAsync();
    }


}
