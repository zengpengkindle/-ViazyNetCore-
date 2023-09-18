using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Core;
using ViazyNetCore.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using ViazyNetCore.Data.FreeSql.Extensions;

namespace ViazyNetCore.Data.FreeSql
{
    [Injection(Lifetime = ServiceLifetime.Singleton)]
    public class CurrentUser : IUser
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public CurrentUser(IHttpContextAccessor accessor, ICurrentUserAccessor currentUserAccessor)
        {
            this._accessor = accessor;
            this._currentUserAccessor = currentUserAccessor;
        }

        public long Id
        {
            get
            {
                return this._accessor?.GetUserId() ?? 0;
            }
        }
        public string Username { get => this._accessor?.GetUserName() ?? string.Empty; }
        public string Nickname { get => this._accessor?.GetNickName() ?? string.Empty; }

        public AuthUserType IdentityType => this._accessor?.GetAuthUserType() ?? AuthUserType.Normal;

        public bool IsModerated => false;

        public long TenantId { get; set; }
    }

    //public class NullCurrentTenant : ICurrentTenant
    //{
    //    public bool IsAvailable { get; }

    //    public int? Id { get; set; }

    //    public string Name { get; set; }

    //    public NullCurrentTenant(int? id, string name = null)
    //    {
    //        this.Id = id;
    //        this.Name = name;
    //        this.IsAvailable = true;
    //    }

    //    public IDisposable Change(int? id, string name = null)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
