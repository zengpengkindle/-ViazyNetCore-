using FreeSql;
using ViazyNetCore.IdleBus;

namespace ViazyNetCore.Repository
{
    public class DefaultIdleRepository<T> : BaseRepository<T> where T : class
    {
        public DefaultIdleRepository(IdleBus<IFreeSql> ib) : base(ib.Get(), null, null) { }
    }


    public class DefaultIdleRepository<T, TKey> : BaseRepository<T, TKey> where T : class
    {
        public DefaultIdleRepository(IdleBus<IFreeSql> ib) : base(ib.Get(), null, null) { }
    }
}
