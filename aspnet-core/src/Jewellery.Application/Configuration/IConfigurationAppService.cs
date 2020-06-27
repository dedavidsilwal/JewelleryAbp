using System.Threading.Tasks;
using Jewellery.Configuration.Dto;

namespace Jewellery.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
