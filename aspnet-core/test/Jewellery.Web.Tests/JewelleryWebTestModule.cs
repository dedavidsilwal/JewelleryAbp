using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Jewellery.EntityFrameworkCore;
using Jewellery.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Jewellery.Web.Tests
{
    [DependsOn(
        typeof(JewelleryWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class JewelleryWebTestModule : AbpModule
    {
        public JewelleryWebTestModule(JewelleryEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JewelleryWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(JewelleryWebMvcModule).Assembly);
        }
    }
}