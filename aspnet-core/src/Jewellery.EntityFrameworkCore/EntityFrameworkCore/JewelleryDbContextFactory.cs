using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Jewellery.Configuration;
using Jewellery.Web;

namespace Jewellery.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class JewelleryDbContextFactory : IDesignTimeDbContextFactory<JewelleryDbContext>
    {
        public JewelleryDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<JewelleryDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            builder.EnableSensitiveDataLogging();
            builder.EnableDetailedErrors();

            JewelleryDbContextConfigurer.Configure(builder, configuration.GetConnectionString(JewelleryConsts.ConnectionStringName));

            return new JewelleryDbContext(builder.Options);
        }
    }
}
