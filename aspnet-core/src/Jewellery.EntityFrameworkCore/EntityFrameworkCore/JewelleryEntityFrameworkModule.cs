using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using Jewellery.EntityFrameworkCore.Seed;
using Microsoft.Extensions.Logging;

namespace Jewellery.EntityFrameworkCore
{
    [DependsOn(
        typeof(JewelleryCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class JewelleryEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public static readonly LoggerFactory MyLoggerFactory
                    = new LoggerFactory(new[]
                    {
                        new Log4NetProvider("log4net.config")
                    });

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<JewelleryDbContext>(options =>
                {
                    options.DbContextOptions.EnableDetailedErrors();
                    options.DbContextOptions.EnableSensitiveDataLogging();

                    options.DbContextOptions.UseLoggerFactory(MyLoggerFactory);

                    if (options.ExistingConnection != null)
                    {
                        JewelleryDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        JewelleryDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(JewelleryEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
