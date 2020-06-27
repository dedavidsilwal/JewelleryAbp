using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Jewellery.EntityFrameworkCore
{
    public static class JewelleryDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<JewelleryDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<JewelleryDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
