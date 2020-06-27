using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Jewellery.Configuration;

namespace Jewellery.Web.Host.Startup
{
    [DependsOn(
       typeof(JewelleryWebCoreModule))]
    public class JewelleryWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public JewelleryWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JewelleryWebHostModule).GetAssembly());
        }
    }
}
