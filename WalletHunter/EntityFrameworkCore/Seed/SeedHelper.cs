using System;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.MultiTenancy;
using SpotifyCache.EntityFrameworkCore.Seed.Host;
using SpotifyCache.EntityFrameworkCore.Seed.Tenants;
using Abp.Extensions;

namespace SpotifyCache.EntityFrameworkCore.Seed
{
    public static class SeedHelper
    {
        public static void SeedHostDb(IIocResolver iocResolver)
        {
            WithDbContext<SpotifyCacheDbContext>(iocResolver, x => SeedHostDb(x));
        }

        public static void SeedHostDb(SpotifyCacheDbContext context, string csvPath = "")
        {
            context.SuppressAutoSetTenantId = true;

            // Host seed
            new InitialHostDbBuilder(context).Create();

            // Default tenant seed (in host database).
            new DefaultTenantBuilder(context).Create();
            new TenantRoleAndUserBuilder(context, 1).Create();
            if (!csvPath.IsNullOrEmpty())
            {
                new SpotifyTracksCreator(context, csvPath).Create();
            }
            new BucketCreator(context).Create();
        }

        private static void WithDbContext<TDbContext>(IIocResolver iocResolver, Action<TDbContext> contextAction)
            where TDbContext : DbContext
        {
            using (var uowManager = iocResolver.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                using (var uow = uowManager.Object.Begin(TransactionScopeOption.Suppress))
                {
                    var context = uowManager.Object.Current.GetDbContext<TDbContext>(MultiTenancySides.Host);

                    contextAction(context);

                    uow.Complete();
                }
            }
        }
    }
}
