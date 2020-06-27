using System.Threading.Tasks;
using Abp.Application.Services;
using Jewellery.Authorization.Accounts.Dto;

namespace Jewellery.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
