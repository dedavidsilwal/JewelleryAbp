using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.EntityFrameworkCore
{
    public static class JewelleryDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<JewelleryDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<JewelleryDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}
