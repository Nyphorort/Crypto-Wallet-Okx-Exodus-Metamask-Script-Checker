using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyCache.Migrator
{
    public class MigratorOptions
    {
        [Option('q', "quiet", Required = false, HelpText = "Run in a non-interactive session")]
        public bool Quiet { get; set; }

        [Option('p', "path", Required = false, HelpText = "Folder path for csv-based seed data")]
        public string Path { get; set; }
    }
}
