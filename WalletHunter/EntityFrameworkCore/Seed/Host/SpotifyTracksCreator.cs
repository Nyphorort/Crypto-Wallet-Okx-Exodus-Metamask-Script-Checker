using CsvHelper;
using EFCore.BulkExtensions;
using SpotifyCache.Domain.Tracks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyCache.EntityFrameworkCore.Seed.Host
{
    public class SpotifyTracksCreator
    {
        private readonly SpotifyCacheDbContext _context;
        private readonly string filepath;

        public SpotifyTracksCreator(SpotifyCacheDbContext context, string path)
        {
            _context = context;
            filepath = Path.Combine(Path.GetFullPath(path), "tracks_features.csv");
        }

        public void Create()
        {
            if (_context.Tracks.Count() > 0) return;
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            _context.BulkInsert(
                csv.GetRecords<Track>().ToList(), 
                new BulkConfig
                {
                    PreserveInsertOrder = false
                });
        }
    }
}
