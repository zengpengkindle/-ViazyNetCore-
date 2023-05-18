using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Core;
using ViazyNetCore.MultiTenancy;
using Microsoft.Extensions.DependencyInjection;

namespace ViazyNetCore.MultiTenancy
{
    [Injection]
    public class CurrentTenant : ICurrentTenant
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual int? Id => _currentTenantAccessor.Current?.TenantId;

        public string Name => _currentTenantAccessor.Current?.Name;

        private readonly ICurrentTenantAccessor _currentTenantAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor)
        {
            _currentTenantAccessor = currentTenantAccessor;
        }

        public IDisposable Change(int? id, string name = null)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(int? tenantId, string name = null)
        {
            var parentScope = _currentTenantAccessor.Current;
            _currentTenantAccessor.Current = new BasicTenantInfo(tenantId, name);
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.Current = parentScope;
            });
        }
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
