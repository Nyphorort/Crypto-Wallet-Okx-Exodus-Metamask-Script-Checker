using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using SpotifyCache.Authorization.Roles;
using SpotifyCache.Authorization.Users;
using SpotifyCache.MultiTenancy;
using SpotifyCache.Analytics;
using SpotifyCache.Domain.Tracks;
using SpotifyCache.Domain;
using SpotifyCache.Analytics.Playlists;

namespace SpotifyCache.EntityFrameworkCore
{
    public class SpotifyCacheDbContext : AbpZeroDbContext<Tenant, Role, User, SpotifyCacheDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<PercentileBucket> PercentileBuckets { get; set; }
        public DbSet<Track> Tracks { get; set; }
        public DbSet<Artist> Artists { get; set; }

        public SpotifyCacheDbContext(DbContextOptions<SpotifyCacheDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //turn off primary key generation for spotify-owned entites!
            modelBuilder.Entity<Track>().Property(x => x.Id).ValueGeneratedNever();
            modelBuilder.Entity<Artist>().Property(x => x.Id).ValueGeneratedNever();
        }
    }
}
