using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Jewellery.Authorization;

namespace Jewellery
{
    [DependsOn(
        typeof(JewelleryCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class JewelleryApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<JewelleryAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(JewelleryApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
