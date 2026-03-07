using EFCore.BulkExtensions;
using SpotifyCache.Analytics;
using SpotifyCache.Analytics.Playlists;
using SpotifyCache.Domain.Tracks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyCache.EntityFrameworkCore.Seed.Host
{
    public class BucketCreator
    {
        private readonly SpotifyCacheDbContext _context;

        public BucketCreator(SpotifyCacheDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.Truncate<PercentileBucket>();
            _context.SaveChanges();
            var totalTracks = _context.Tracks.Count();
            if (totalTracks < 100) return; //not enough data to calculate accurate buckets
            var allBuckets = new List<PercentileBucket>();
            foreach(var stat in Enum.GetValues(typeof(Statistic)).Cast<Statistic>())
            {
                allBuckets.AddRange(CreateStatBucket(stat, totalTracks));
            }
            _context.PercentileBuckets.AddRange(allBuckets);

            _context.Playlists.AddRange(CreatePlaylists());
            _context.SaveChanges();
        }

        private List<PercentileBucket> CreateStatBucket(Statistic stat, int totalTracks, int buckets = 20)
        {
            var results = new List<PercentileBucket>();
            var expression = stat.ToExpression();
            var initialQuery = _context.Tracks.AsQueryable()
                    .Select(expression)
                    .OrderBy(x => x)
                    .ToList();

            var tracksPerBucket = totalTracks / buckets;

            var min = initialQuery.First();
            for(var i = 0; i < buckets; i++)
            {
                var max = initialQuery.Skip(((i + 1) * tracksPerBucket) - 1).First();
                results.Add(new PercentileBucket
                {
                    Min = min,
                    Max = max,
                    Statistic = stat,
                    Count = i
                });
                min = max;
            }
            return results;
        }

        private List<Playlist> CreatePlaylists(int uniqueTracks = 20)
        {
            var results = new List<Playlist>();
            var rng = new Random();
            var trackIds = _context.Tracks
                .Select(x => x.Id)
                .Skip(rng.Next(0, _context.Tracks.Count() - uniqueTracks))
                .Take(uniqueTracks);
            foreach (var id in trackIds)
            {
                var limit = rng.Next(1, 10);
                for (int j = 0; j < limit; j++)
                {
                    var reactions = rng.Next(1, 5);
                    results.Add(new Playlist
                    {
                        TrackId = id,
                        LikeCount = reactions,
                        DislikeCount = reactions
                    });
                }
            }
            return results;
        }
    }
}
