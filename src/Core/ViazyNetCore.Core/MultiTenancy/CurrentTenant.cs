using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Core;
using ViazyNetCore.MultiTenancy;

namespace ViazyNetCore.MultiTenancy
{
    public class CurrentTenant : ICurrentTenant
    {
        public virtual bool IsAvailable => Id.HasValue;

        public virtual long? Id => _currentTenantAccessor.Current?.TenantId;

        public string Name => _currentTenantAccessor.Current?.Name;

        private readonly ICurrentTenantAccessor _currentTenantAccessor;

        public CurrentTenant(ICurrentTenantAccessor currentTenantAccessor)
        {
            _currentTenantAccessor = currentTenantAccessor;
        }

        public IDisposable Change(long? id, string name = null)
        {
            return SetCurrent(id, name);
        }

        private IDisposable SetCurrent(long? tenantId, string name = null)
        {
            var parentScope = _currentTenantAccessor.Current;
            _currentTenantAccessor.Current = new BasicTenantInfo(tenantId, name);
            return new DisposeAction(() =>
            {
                _currentTenantAccessor.Current = parentScope;
            });
        }
    }
}
