using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using SpotifyCache.Configuration;
using SpotifyCache.EntityFrameworkCore;
using SpotifyCache.Migrator.DependencyInjection;

namespace SpotifyCache.Migrator
{
    [DependsOn(typeof(SpotifyCacheEntityFrameworkModule))]
    public class SpotifyCacheMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public SpotifyCacheMigratorModule(SpotifyCacheEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(SpotifyCacheMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                SpotifyCacheConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(SpotifyCacheMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
