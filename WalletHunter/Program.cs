using System;
using Castle.Facilities.Logging;
using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Collections.Extensions;
using Abp.Dependency;
using CommandLine;
using System.Collections.Generic;
using System.Linq;

namespace SpotifyCache.Migrator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var exitCode = Parser.Default.ParseArguments<MigratorOptions>(args)
                .MapResult(BootstrapAndRunAbp, HandleParseError);
            Environment.Exit(exitCode);
        }

        static int BootstrapAndRunAbp(MigratorOptions options)
        {
            using var bootstrapper = AbpBootstrapper.Create<SpotifyCacheMigratorModule>();
            bootstrapper.IocManager.IocContainer
                .AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );

            bootstrapper.Initialize();

            using var migrateExecuter = bootstrapper.IocManager.ResolveAsDisposable<MultiTenantMigrateExecuter>();
            var migrationSucceeded = migrateExecuter.Object.Run(options);
            if(options.Quiet)
            {
                return Convert.ToInt32(!migrationSucceeded);
            }
            if (!migrationSucceeded)
            {
                Console.WriteLine("Migration failed.");
            }
            Console.WriteLine("Press ENTER to exit");
            Console.ReadLine();
            return 0;
        }

        //in case of errors or --help or --version
        static int HandleParseError(IEnumerable<Error> errs)
        {
            var result = -2;
            Console.WriteLine("errors {0}", errs.Count());
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            return result;
        }
    }
}
