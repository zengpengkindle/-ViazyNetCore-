using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.MultiTenancy
{
    public class AsyncLocalCurrentUserAccessor : ICurrentUserAccessor
    {
        public static AsyncLocalCurrentUserAccessor Instance { get; } = new();

        public IUser Current
        {
            get => _currentScope.Value;
            set => _currentScope.Value = value;
        }

        private readonly AsyncLocal<IUser> _currentScope;

        private AsyncLocalCurrentUserAccessor()
        {
            _currentScope = new AsyncLocal<IUser>();
        }
    }
}
