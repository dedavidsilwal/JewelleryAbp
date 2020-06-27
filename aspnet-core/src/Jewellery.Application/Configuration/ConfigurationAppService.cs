using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using Jewellery.Configuration.Dto;

namespace Jewellery.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : JewelleryAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
