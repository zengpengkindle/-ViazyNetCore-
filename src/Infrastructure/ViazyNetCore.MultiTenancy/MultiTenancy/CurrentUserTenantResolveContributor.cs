using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.MultiTenancy
{
    public class CurrentUserTenantResolveContributor : TenantResolveContributorBase
    {
        public const string ContributorName = "CurrentUser";

        public override string Name => ContributorName;

        public override Task ResolveAsync(ITenantResolveContext context)
        {
            var currentUser = context.ServiceProvider.GetRequiredService<IUser>();
            if (currentUser.Id > 0)
            {
                context.Handled = true;
                context.TenantIdOrName = currentUser.TenantId.ToString();
            }

            return Task.CompletedTask;
        }
    }
}
