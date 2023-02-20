using FreeSql;
using ViazyNetCore.IdleBus;

namespace ViazyNetCore.Repository
{
    public class DefaultIdlsRepository<T> : BaseRepository<T> where T : class
    {
        public DefaultIdlsRepository(IdleBus<IFreeSql> ib) : base(ib.Get(), null, null) { }
    }


    public class DefaultIdlsRepository<T, TKey> : BaseRepository<T, TKey> where T : class
    {
        public DefaultIdlsRepository(IdleBus<IFreeSql> ib) : base(ib.Get(), null, null) { }
    }
}
