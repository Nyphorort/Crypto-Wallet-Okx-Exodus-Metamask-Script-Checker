using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace SpotifyCache.EntityFrameworkCore
{
    public static class SpotifyCacheDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<SpotifyCacheDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<SpotifyCacheDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
