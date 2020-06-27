using System.Threading.Tasks;
using Abp.Application.Services;
using Jewellery.Sessions.Dto;

namespace Jewellery.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
