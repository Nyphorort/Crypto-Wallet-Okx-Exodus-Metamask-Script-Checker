using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using SpotifyCache.Configuration;
using SpotifyCache.Web;

namespace SpotifyCache.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class SpotifyCacheDbContextFactory : IDesignTimeDbContextFactory<SpotifyCacheDbContext>
    {
        public SpotifyCacheDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SpotifyCacheDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            SpotifyCacheDbContextConfigurer.Configure(builder, configuration.GetConnectionString(SpotifyCacheConsts.ConnectionStringName));

            return new SpotifyCacheDbContext(builder.Options);
        }
    }
}
